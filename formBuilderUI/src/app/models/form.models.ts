export type FieldType =
  | 'text'
  | 'textarea'
  | 'number'
  | 'date'
  | 'datetime'
  | 'email'
  | 'phone'
  | 'select'
  | 'radio'
  | 'checkbox'
  | 'file';

export type ActionType = 'Approve' | 'Reject' | 'ApproveReject';

export interface FieldTypeDefinition {
  type: FieldType;
  label: string;
  hasOptions: boolean;
}

export const FIELD_TYPE_DEFINITIONS: readonly FieldTypeDefinition[] = [
  { type: 'text', label: 'טקסט', hasOptions: false },
  { type: 'textarea', label: 'טקסט מרובה שורות', hasOptions: false },
  { type: 'number', label: 'מספר', hasOptions: false },
  { type: 'date', label: 'תאריך', hasOptions: false },
  { type: 'datetime', label: 'תאריך ושעה', hasOptions: false },
  { type: 'email', label: 'אימייל', hasOptions: false },
  { type: 'phone', label: 'טלפון', hasOptions: false },
  { type: 'select', label: 'רשימה נפתחת', hasOptions: true },
  { type: 'radio', label: 'בחירה יחידה', hasOptions: true },
  { type: 'checkbox', label: 'תיבת סימון', hasOptions: false },
  { type: 'file', label: 'העלאת קובץ', hasOptions: false }
];

export function fieldTypeLabel(type: FieldType): string {
  return FIELD_TYPE_DEFINITIONS.find((d) => d.type === type)?.label ?? type;
}

export function fieldTypeHasOptions(type: FieldType): boolean {
  return FIELD_TYPE_DEFINITIONS.find((d) => d.type === type)?.hasOptions ?? false;
}

export interface FormField {
  label: string;
  type: FieldType;
  order: number;
  required: boolean;
  options?: string[];
}

export interface ApprovalStep {
  name: string;
  order: number;
  approver: string;
  actionType: ActionType;
}

export interface CreateFormRequest {
  name: string;
  createdBy: string;
  fields: FormField[];
  approvalSteps: ApprovalStep[];
}

export interface FormFieldResponse extends FormField {
  id: number;
}

export interface ApprovalStepResponse extends ApprovalStep {
  id: number;
}

export interface FormTemplate {
  id: number;
  name: string;
  createdAt: string;
  createdBy: string;
  fields: FormFieldResponse[];
  approvalSteps: ApprovalStepResponse[];
}
