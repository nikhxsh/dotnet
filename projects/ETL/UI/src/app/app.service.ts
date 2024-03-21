import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Column, DbSchemaRequest, DbSchemaResponse, RequestStatus } from './app.model';

@Injectable()
export class MigrationService {

    constructor(private http: HttpClient) {
    }

    public fetchSchema(request: DbSchemaRequest): Observable<DbSchemaResponse[]> {
        return this.http.post<any>(`https://localhost:44386/api/sqltomongo/schema`, request)
            .pipe(
                map(response => response.map((res: { Table: any; Columns: any[]; }):DbSchemaResponse => ({
                    table: res.Table,
                    tableKey: "",
                    columns: res.Columns.map(x => new Column({name: x.Name, dataType: x.DataType})),
                    selectedColumns: new Array(res.Columns.length),
                    mainCheck: false,
                    nestedCheck: false,
                    objectName: "",
                    sourceId:"",
                    targetTable: "",
                    targetId: ""
                })))
            );
    }

    
    public fetchSampleDocument(request: any): Observable<any> {
        return this.http.post<any>(`https://localhost:44386/api/sqltomongo/sample`, request)
            .pipe(
                map((response: any) => response)
            );
    }

    public migrateSqlToMongo(request: any): Observable<any> {
        return this.http.post(`https://localhost:44386/api/sqltomongo/migrate`, request)
            .pipe(
                map((response: any) => response)
            );
    }

    public migrateSqlToPostgre(request: any): Observable<any> {
        return this.http.post(`https://localhost:44386/api/sqltopostgre/migrate`, request)
            .pipe(
                map((response: any) => response)
            );
    }

    public status(requestId: any): Observable<any> {
        return this.http.get(`https://localhost:44386/api/sqltopostgre/status?requestId=${requestId}`)
            .pipe(
                map((response: any) => new RequestStatus({requestId : response.RequestId, count: response.Count, status: response.Status }))
            );
    }
}