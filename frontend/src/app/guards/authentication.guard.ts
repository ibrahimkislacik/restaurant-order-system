import {Injectable} from "@angular/core";
import {
    ActivatedRouteSnapshot,
    CanActivate,
    Router,
    RouterStateSnapshot
} from "@angular/router";
import {StorageService} from "../services/storage.service";

@Injectable()
export class AuthenticationGuard implements CanActivate {
    constructor(
        private router: Router,
        private storageService: StorageService) {
    }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.storageService.user) {

            if (route.data['permissions']) {
                if (route.data['permissions'].indexOf(
                    "Admin") !== -1 && !this.storageService.user.isAdmin) {
                    this.router.navigate(["/dashboard"]);
                    return false;
                }
                else if (route.data['permissions'].indexOf(
                    "User") !== -1 && this.storageService.user.isAdmin) {
                    this.router.navigate(["/dashboard"]);
                    return false;
                }
            }
            return true;
        }

        this.router.navigate(["/auth/signin"]);
        return false;
    }
}
