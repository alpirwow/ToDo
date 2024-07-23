export interface ToDoItemPutDto {
    id: string;
    title?: string;
    description?: string;
    isCompleted?: boolean;
    dueDate?: string;
    priorityLevel?: number;
    userId: string;
}