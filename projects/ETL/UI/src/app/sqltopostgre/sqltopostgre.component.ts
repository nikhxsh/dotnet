import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { FormField } from '@nikxsh/ngmodelform';
import { ColumnMapper, DbSchemaRequest, DbSchemaResponse, Settings, SQLToPostgreTemplate, Table, TableSelection, TargetTable, TemplateSettings, RequestStatus } from '../app.model';
import { MigrationService } from '../app.service';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-sqltopostgre',
  templateUrl: './sqltopostgre.component.html',
  encapsulation: ViewEncapsulation.None
})
export class SqltopostgreComponent implements OnInit {
  sqlFormFields: FormField[] = [];
  postgreFormFields: FormField[] = [];
  template: SQLToPostgreTemplate = new SQLToPostgreTemplate();
  dbSchema: DbSchemaResponse[] = []
  displayTemplate = ""
  targetColumns: TableSelection = new TableSelection({ name: "", columns: [] })
  tableSelection: TableSelection[] = []
  migrationResponse: any
  requestStatus : RequestStatus = new RequestStatus()

  constructor(private MigrationServiceRef: MigrationService) {
    this.template.mainTable = new Table({});
    this.updateTempate();
  }

  ngOnInit(): void {
    this.sqlFormFields = [
      new FormField({ label: "MSSQL Connection" }).TextField({
        name: "sqlDbUrl",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "MSSQL Databse Name" }).TextField({
        name: "sqlDbName",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "Filter" }).TextField({
        name: "filterTable",
        control: new FormControl('')
      })
    ];

    this.postgreFormFields = [
      new FormField({ label: "PostgreSQL Connection" }).TextField({
        name: "postgreDbUrl",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "PostgreSQL Databse Name" }).TextField({
        name: "postgreDbName",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "Target Tables" }).SelectField({
        name: "targetTables",
        control: new FormControl('', [Validators.required])
      },
        [1, 2, 3, 4, 5]),
      new FormField({ label: "Transfer Size" }).SelectField({
        name: "transferSize",
        control: new FormControl('', [Validators.required])
      },
      [10, 20, 50, 100, 1000, 10000])
    ];
    this.updateTempate();
  }

  public getDbSchema(event: any) {
    let sqlSetting = new Settings({ connection: event.sqlDbUrl, database: event.sqlDbName });
    this.template.settings = new TemplateSettings({ sql: sqlSetting })

    this.updateTempate();

    let schemaRequest = new DbSchemaRequest({ url: sqlSetting.connection, dbName: sqlSetting.database, filter: event.filterTable });

    this.MigrationServiceRef.fetchSchema(schemaRequest)
      .subscribe(schemaInfo => {
        this.dbSchema = schemaInfo
      });
  }

  onMainTableSelect(table: string, index: number) {
    this.dbSchema[index].selectedColumns = [];

    let mainTable = this.dbSchema.find((x: { table: string; }) => x.table == table);
    if (mainTable) {
      this.targetColumns.columns = mainTable!.columns.map(x => x);
      this.template.mainTable.select = this.dbSchema.find(x => x.table == table)?.columns.map(x => x.name);
    }
    this.template.mainTable!.tableName = table;
    const lastMainTableIndex = this.dbSchema.findIndex(x => x.table == this.template.mainTable!.tableName);
    if (lastMainTableIndex > -1) {
      this.dbSchema[lastMainTableIndex].selectedColumns = [];
    }
    this.updateTempate();
  }

  onPostgreSettingUpdate(event: any) {
    let postgreSetting = new Settings({ connection: event.postgreDbUrl, database: event.postgreDbName });
    this.template.settings = new TemplateSettings({ postgres: postgreSetting, sql: this.template.settings?.sql })
    this.template.transferSize = +event.transferSize
    this.tableSelection = [];
    this.template.targetTables = [];
    for (let i = 0; i < event.targetTables; i++) {
      this.tableSelection.push(new TableSelection({ name: `Table${i}`, columns: [] }))
      this.template.targetTables.push({tableName: `Table${i}`, columnMappings: []})
    }
    this.updateTempate();
  }

  drop(event: CdkDragDrop<TableSelection>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data.columns, event.previousIndex, event.currentIndex);
    } else {
      console.log('Transfering item to new container')
      transferArrayItem(event.previousContainer.data.columns,
        event.container.data.columns,
        event.previousIndex,
        event.currentIndex);
      let targetTable = this.template.targetTables.find(x => x.tableName == event.container.data.name);
      targetTable.columnMappings = event.container.data.columns.map(x => new ColumnMapper({source: x.name, target: x.name, dataType: x.dataType}));
    }    
    this.updateTempate();
  }

  updateTempate() {
    this.displayTemplate = JSON.stringify(this.template, undefined, 4);
  }  

  public migrateData() {
    this.requestStatus.status = "Migration Started..."
    this.MigrationServiceRef.migrateSqlToPostgre(JSON.parse(this.displayTemplate))
      .subscribe(response => {
        this.requestStatus.requestId = response.RequestId;
        this.requestStatus.status = response.Message;
      });
  }

  public getStatus() {
    this.MigrationServiceRef.status(this.requestStatus.requestId)
      .subscribe(response => {
        this.requestStatus.requestId = response.requestId;
        this.requestStatus.count = response.count;
        this.requestStatus.status = response.status;
      });
  }
}
