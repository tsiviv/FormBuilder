export type FieldType = 'text' | 'date';

export type ActionType = 'Approve' | 'Reject' | 'ApproveReject';

export interface FormField {
  label: string;
  type: FieldType;
  order: number;
  required: boolean;
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
