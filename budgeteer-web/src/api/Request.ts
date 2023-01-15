import { authStore } from "../stores/AuthStore";

export async function getWithAuth(url: string): Promise<Response> {
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authStore.currentAuthUser?.id_token}`
        }
    });
    
    return response
}

export async function putWithAuth(url: string, body: any): Promise<Response> {
    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authStore.currentAuthUser?.id_token}`
        },
        body: JSON.stringify(body)
    });
    
    return response
}

export async function postWithAuth(url: string, body: any): Promise<Response> {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authStore.currentAuthUser?.id_token}`
        },
        body: JSON.stringify(body)
    });
    
    return response
}

export async function deleteWithAuth(url: string, body?: any): Promise<Response> {
    const response = await fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authStore.currentAuthUser?.id_token}`
        },
        body: body ? JSON.stringify(body) : undefined
    });
    
    return response
}