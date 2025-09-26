module nodak.models {
    export class User {
        username: string;
        password: string;
        organizationId: any;
        firstName: string;
        lastName: string;
        token: string;
        rememberMe: boolean;
    }
}