import { ApiEndpoint } from "../constants/ApiConstants";
import { IUserProfile } from "../models/UserProfile";
import { authStore } from "../stores/AuthStore";

const userProfileEndpoint = ApiEndpoint('/v1/user/profile');

export default class UserAPI {
    static async getUserProfile(): Promise<IUserProfile> {
        const response = await fetch(userProfileEndpoint, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${authStore.currentAuthUser?.id_token}`
            }
        });
    
        if (response.ok) {
            return await response.json();
        } else {
            throw new Error('Failed to get user profile');
        }
    }
}
