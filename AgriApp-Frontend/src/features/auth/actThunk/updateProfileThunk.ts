import { createAsyncThunk } from "@reduxjs/toolkit";
import { updateMyProfile } from "../services/profileService";
import type { IUser } from "../types/authDto";
import type { IUserProfile } from "../types/IUserProfile";

// updateProfileThunk.ts
export const updateProfileThunk = createAsyncThunk<
  IUser,
  IUserProfile,
  { rejectValue: string }
>(
  "profile/updateMyProfile",
  async (
    { fullName, username, phone, imageFile }: IUserProfileToUpdate,
    thunkAPI,
  ) => {
    try {
      const formData = new FormData();

      formData.append("fullName", fullName);
      formData.append("username", username);
      formData.append("phone", phone ?? "");
      if (imageFile && imageFile.length > 0) {
        formData.append("imageFile", imageFile[0]);
      }

      console.log(formData);
      console.log(Object.fromEntries(formData.entries()));

      return await updateMyProfile(formData);
    } catch {
      return thunkAPI.rejectWithValue("Profile update failed");
    }
  },
);
