import { ApiEndpoint } from "../constants/ApiConstants";
import { IInterest } from "../models/Interest";
import { getWithAuth } from "./Request";

const allInterestsEndpoint = ApiEndpoint("/v1/interest/all");
const addInterestEndpoint = ApiEndpoint("/v1/interest/add/");
const removeInterestsEndpoint = ApiEndpoint("/v1/interest/remove/");

export default class UserAPI {
  static async getAllInterests(): Promise<IInterest[]> {
    const response = await getWithAuth(allInterestsEndpoint);

    if (!response.ok) {
      throw new Error(`Failed to get all interests: ${await response.text()}`);
    }

    return await response.json();
  }

  static async addInterest(name: string) {
    const response = await getWithAuth(addInterestEndpoint + name);

    if (!response.ok) {
      throw new Error("Failed to add interest: " + (await response.text()));
    }

    return await response.json();
  }

  static async removeInterest(name: string) {
    const response = await getWithAuth(removeInterestsEndpoint + name);

    if (!response.ok) {
      throw new Error("Failed to remove interest: " + (await response.text()));
    }

    return await response.json();
  }
}
