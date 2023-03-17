export const API_BASE_URL = process.env.NODE_ENV === 'development' ? 'http://localhost:5116' : 'https://threaditapi.noahc3.cc';

export function ApiEndpoint(endpoint: string) {
    return API_BASE_URL + endpoint;
}