export interface UserResponseDto {
  Id: number;
  fullName: string;
  Username: string;
  Email: string;
  UserType: number;
  ImagesFile?: string;
  CreatedAt: Date;
}
