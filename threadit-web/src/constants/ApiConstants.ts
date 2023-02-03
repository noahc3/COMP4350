export const API_BASE_URL = 'http://localhost:5116';

export function ApiEndpoint(endpoint: string) {
    return API_BASE_URL + endpoint;
}