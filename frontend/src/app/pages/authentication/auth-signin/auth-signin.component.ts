import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterModule} from '@angular/router';
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {SharedModule} from "../../../theme/shared/shared.module";
import {LoginService} from "../../../services/login.service";
import {ToastService} from "../../../services/toast-service";
import {LoginRequestModel} from "../../../models/login-request.model";

@Component({
    selector: 'app-auth-signin',
    standalone: true,
    imports: [RouterModule, SharedModule],
    templateUrl: './auth-signin.component.html',
    styleUrls: ['./auth-signin.component.scss'],
})
export default class AuthSigninComponent implements OnInit {
    loginForm!: UntypedFormGroup;
    isSubmitted: boolean = false;
    isButtonSpinnerActive: boolean = false;

    constructor(private formBuilder: UntypedFormBuilder, private router: Router, private route: ActivatedRoute,
                private loginService: LoginService, public toastService: ToastService) {
    }

    ngOnInit(): void {
        this.loginForm = this.formBuilder.group({
            eMail: [null, Validators.compose([Validators.required, Validators.email])],
            password: ['', Validators.required],
        });
    }

    get f() {
        return this.loginForm.controls;
    }

    async onSubmit() {
        this.isSubmitted = true;

        if (this.loginForm.invalid) {
            return;
        }
        let request: LoginRequestModel = new LoginRequestModel();
        request.eMail = this.f['eMail'].value;
        request.password = this.f['password'].value;
        this.isButtonSpinnerActive = true;
        this.loginService.login(request).subscribe({
            next: response => {
                this.isButtonSpinnerActive = false;
                this.router.navigate(['/']);
            },
            error: async error => {
                this.isButtonSpinnerActive = false;
                this.toastService.fake();
                document.getElementById('eMail').focus();
                throw error;
            }
        });
    }


}
