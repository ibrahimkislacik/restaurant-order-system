import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import {SharedModule} from "../../../theme/shared/shared.module";
import AuthSigninComponent from "./auth-signin/auth-signin.component";

@NgModule({
  declarations: [],
  imports: [CommonModule, SharedModule, AuthenticationRoutingModule],
})
export class AuthenticationModule {}
