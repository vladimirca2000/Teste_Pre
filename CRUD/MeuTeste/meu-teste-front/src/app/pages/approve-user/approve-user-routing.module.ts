import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApproveUserComponent } from './approve-user.component';

const routes: Routes = [
  {
    path: '',
    component: ApproveUserComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApproveUserRoutingModule { }
