import { ApiEndpoint } from "../constants/ApiConstants";
import { IUserProfile } from "../models/UserProfile";
import { getWithAuth } from "./Request";

const userProfileEndpoint = ApiEndpoint('/v1/user/profile');

export default class UserAPI {
    static async getUserProfile(): Promise<IUserProfile> {
        const response = await getWithAuth(userProfileEndpoint);
    
        if (!response.ok) {
            throw new Error(`Failed to get user profile: ${await response.text()}`);
        }

        return await response.json();
    }
}
