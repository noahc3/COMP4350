export async function fakeApiDelay() {
    return new Promise(resolve => setTimeout(resolve, Math.random() * 2000 + 500));
}