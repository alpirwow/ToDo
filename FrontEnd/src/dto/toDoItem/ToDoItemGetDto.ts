export interface ToDoItemGetDto {
    id: string;
    userId: string;
    title: string;
    description?: string;
    isCompleted: boolean;
    dueDate: string;
    priorityLevel: number;
}