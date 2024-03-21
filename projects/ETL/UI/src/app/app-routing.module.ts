import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SqltomongoComponent } from './sqltomongo/sqltomongo.component';
import { SqltopostgreComponent } from './sqltopostgre/sqltopostgre.component';

const routes: Routes = [
	{ path: '', redirectTo: 'app', pathMatch: 'full' },
	{ path: 'sqltomongo', component: SqltomongoComponent },
	{ path: 'sqltopostgre', component: SqltopostgreComponent },
	{ path: '**', component: SqltomongoComponent } //catch all route by using the path **
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
