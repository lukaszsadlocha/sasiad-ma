export interface BorrowRequest {
  id: string;
  itemId: string;
  itemName: string;
  itemImageUrl?: string;
  borrowerId: string;
  borrowerName: string;
  borrowerProfileImageUrl?: string;
  lenderId: string;
  lenderName: string;
  status: BorrowStatus;
  requestDate: string;
  requestedFromDate: string;
  requestedToDate: string;
  approvedDate?: string;
  borrowedDate?: string;
  returnedDate?: string;
  notes?: string;
  lenderNotes?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateBorrowRequest {
  itemId: string;
  requestedFromDate: string;
  requestedToDate: string;
  notes?: string;
}

export interface UpdateBorrowRequest {
  status: BorrowStatus;
  lenderNotes?: string;
  borrowedDate?: string;
  returnedDate?: string;
}

export interface BorrowRequestDto {
  id: string;
  itemId: string;
  itemName: string;
  itemImageUrl?: string;
  borrowerId: string;
  borrowerName: string;
  borrowerEmail: string;
  borrowerPhoneNumber?: string;
  borrowerProfileImageUrl?: string;
  lenderId: string;
  lenderName: string;
  status: BorrowStatus;
  requestDate: string;
  requestedFromDate: string;
  requestedToDate: string;
  approvedDate?: string;
  borrowedDate?: string;
  returnedDate?: string;
  notes?: string;
  lenderNotes?: string;
  canApprove: boolean;
  canReturn: boolean;
  canCancel: boolean;
  createdAt: string;
}

export enum BorrowStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Borrowed = 'Borrowed',
  Returned = 'Returned',
  Cancelled = 'Cancelled',
  Declined = 'Declined'
}

export interface BorrowHistory {
  totalBorrows: number;
  totalLends: number;
  successfulReturns: number;
  pendingRequests: number;
  activeBorrows: number;
  recentRequests: BorrowRequestDto[];
}
