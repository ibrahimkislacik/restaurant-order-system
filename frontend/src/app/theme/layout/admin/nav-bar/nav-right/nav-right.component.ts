// angular import
import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {LoginService} from "../../../../../services/login.service";

@Component({
    selector: 'app-nav-right',
    templateUrl: './nav-right.component.html',
    styleUrls: ['./nav-right.component.scss'],
})
export class NavRightComponent {

    constructor(private router: Router, private loginService: LoginService) {

    }

    logout() {
        this.loginService.logout();
        this.router.navigate(["/auth/signin"]).then();
    }
}
