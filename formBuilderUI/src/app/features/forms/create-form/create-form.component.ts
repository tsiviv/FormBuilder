import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';

import { FormsService } from '../../../services/forms.service';
import {
  ActionType,
  CreateFormRequest,
  FIELD_TYPE_DEFINITIONS,
  FieldType,
  fieldTypeHasOptions,
  fieldTypeLabel
} from '../../../models/form.models';

type FieldGroup = FormGroup<{
  label: FormControl<string>;
  type: FormControl<FieldType>;
  required: FormControl<boolean>;
  order: FormControl<number>;
  options: FormArray<FormControl<string>>;
}>;

type ApprovalStepGroup = FormGroup<{
  name: FormControl<string>;
  approver: FormControl<string>;
  actionType: FormControl<ActionType>;
  order: FormControl<number>;
}>;

const DEFAULT_CREATED_BY = 'admin';
const ACTION_TYPES: ActionType[] = ['Approve', 'Reject', 'ApproveReject'];

function requiredNotBlank(control: AbstractControl<string>): ValidationErrors | null {
  return control.value && control.value.trim().length > 0 ? null : { required: true };
}

function optionsRequiredForChoiceTypes(control: AbstractControl): ValidationErrors | null {
  const group = control as FieldGroup;
  if (!fieldTypeHasOptions(group.controls.type.value)) {
    return null;
  }
  const hasNonEmptyOption = group.controls.options.controls.some((c) => c.value.trim().length > 0);
  return hasNonEmptyOption ? null : { optionsRequired: true };
}

@Component({
  selector: 'app-create-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-form.component.html',
  styleUrl: './create-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateFormComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly formsService = inject(FormsService);

  protected readonly fieldTypes = FIELD_TYPE_DEFINITIONS;
  protected readonly actionTypes = ACTION_TYPES;

  protected readonly isSaving = signal(false);
  protected readonly submitted = signal(false);
  protected readonly successMessage = signal<string | null>(null);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly form = this.fb.group({
    name: this.fb.control('', [requiredNotBlank, Validators.maxLength(200)]),
    createdBy: this.fb.control(DEFAULT_CREATED_BY, [Validators.required, Validators.maxLength(200)]),
    fields: this.fb.array<FieldGroup>([], Validators.required),
    approvalSteps: this.fb.array<ApprovalStepGroup>([], Validators.required)
  });

  get fields(): FormArray<FieldGroup> {
    return this.form.controls.fields;
  }

  get approvalSteps(): FormArray<ApprovalStepGroup> {
    return this.form.controls.approvalSteps;
  }

  typeLabel(type: FieldType): string {
    return fieldTypeLabel(type);
  }

  fieldHasOptions(field: FieldGroup): boolean {
    return fieldTypeHasOptions(field.controls.type.value);
  }

  addField(type: FieldType): void {
    const group: FieldGroup = this.fb.group(
      {
        label: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
        type: this.fb.control<FieldType>(type),
        required: this.fb.control(false),
        order: this.fb.control(this.fields.length + 1),
        options: this.fb.array<string>([])
      },
      { validators: optionsRequiredForChoiceTypes }
    );

    if (fieldTypeHasOptions(type)) {
      group.controls.options.push(this.fb.control(''));
      group.controls.options.push(this.fb.control(''));
    }

    this.fields.push(group);
  }

  removeField(index: number): void {
    this.fields.removeAt(index);
    this.updateOrder(this.fields);
  }

  addOption(fieldIndex: number): void {
    this.fields.at(fieldIndex).controls.options.push(this.fb.control(''));
  }

  removeOption(fieldIndex: number, optionIndex: number): void {
    this.fields.at(fieldIndex).controls.options.removeAt(optionIndex);
  }

  addApprovalStep(): void {
    const group: ApprovalStepGroup = this.fb.group({
      name: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
      approver: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
      actionType: this.fb.control<ActionType>('Approve', [Validators.required]),
      order: this.fb.control(this.approvalSteps.length + 1)
    });
    this.approvalSteps.push(group);
  }

  removeApprovalStep(index: number): void {
    this.approvalSteps.removeAt(index);
    this.updateOrder(this.approvalSteps);
  }

  save(): void {
    if (this.isSaving()) return;

    this.successMessage.set(null);
    this.errorMessage.set(null);
    this.submitted.set(true);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.errorMessage.set('נא לתקן את השגיאות בטופס לפני השמירה');
      return;
    }

    const request = this.buildRequest();

    this.isSaving.set(true);
    this.formsService.createForm(request).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.successMessage.set('הטופס נשמר בהצלחה');
        this.resetForm();
      },
      error: (err: HttpErrorResponse) => {
        this.isSaving.set(false);
        this.errorMessage.set(this.extractErrorMessage(err));
      }
    });
  }

  private updateOrder(array: FormArray<FieldGroup> | FormArray<ApprovalStepGroup>): void {
    array.controls.forEach((control, index) => control.controls.order.setValue(index + 1));
  }

  private buildRequest(): CreateFormRequest {
    return {
      name: this.form.controls.name.value.trim(),
      createdBy: this.form.controls.createdBy.value.trim() || DEFAULT_CREATED_BY,
      fields: this.fields.controls.map((group) => {
        const type = group.controls.type.value;
        const options = fieldTypeHasOptions(type)
          ? group.controls.options.controls.map((c) => c.value.trim()).filter((v) => v.length > 0)
          : undefined;

        return {
          label: group.controls.label.value.trim(),
          type,
          order: group.controls.order.value,
          required: group.controls.required.value,
          ...(options ? { options } : {})
        };
      }),
      approvalSteps: this.approvalSteps.controls.map((group) => ({
        name: group.controls.name.value.trim(),
        approver: group.controls.approver.value.trim(),
        actionType: group.controls.actionType.value,
        order: group.controls.order.value
      }))
    };
  }

  private resetForm(): void {
    while (this.fields.length) {
      this.fields.removeAt(0);
    }
    while (this.approvalSteps.length) {
      this.approvalSteps.removeAt(0);
    }
    this.form.reset({ name: '', createdBy: DEFAULT_CREATED_BY });
    this.submitted.set(false);
  }

  private extractErrorMessage(err: HttpErrorResponse): string {
    if (err.status === 0) {
      return 'לא ניתן להתחבר לשרת. ודא שה-API פעיל ונסה שוב.';
    }

    const body: unknown = err.error;
    if (body && typeof body === 'object' && 'errors' in body) {
      const errors = (body as { errors: Record<string, string[]> }).errors;
      const messages = Object.values(errors).flat();
      if (messages.length) {
        return messages.join(' | ');
      }
    }

    if (body && typeof body === 'object' && 'title' in body) {
      const title = (body as { title: unknown }).title;
      if (typeof title === 'string') {
        return title;
      }
    }

    return `שמירת הטופס נכשלה (שגיאה ${err.status})`;
  }
}
