class DbSchemaRequest {
	url?: string
	dbName?: string
	filter?: string

    public constructor(init?:Partial<DbSchemaRequest>) {
        Object.assign(this, init);
    }
}

interface DbSchemaResponse {
	table: string
	tableKey: string
	columns: Column[]
	selectedColumns: string[]
	mainCheck: boolean
	nestedCheck: boolean
	objectName: string
	sourceId: string
	targetTable: string
	targetId: string
}

class Column {
	name: string
	dataType: string

    public constructor(init?:Partial<Column>) {
        Object.assign(this, init);
    }
}

class TemplateSettings {
	sql?: Settings
	mongo?: Settings
	postgres?: Settings

    public constructor(init?:Partial<TemplateSettings>) {
        Object.assign(this, init);
    }
}

class SQLToMongoTemplate {
	settings?: TemplateSettings
	mainTable?: Table
	nestedTables?: NestedTable[]
	transferSize?: number = 0

    public constructor(init?:Partial<SQLToMongoTemplate>) {
        Object.assign(this, init);
    }
}

class SQLToPostgreTemplate {
	settings?: TemplateSettings
	mainTable?: Table
	targetTables?: TargetTable[]
	transferSize?: number = 0

    public constructor(init?:Partial<SQLToPostgreTemplate>) {
        Object.assign(this, init);
    }
}

class Settings {
	connection?: string
	database?: string
	collection?: string

    public constructor(init?:Partial<Settings>) {
        Object.assign(this, init);
    }
}

class Table {
	key?: string
	tableName?: string
	select?: string[]
	conditions?: string

    public constructor(init?:Partial<Table>) {
        Object.assign(this, init);
    }
}

class NestedTable {
	tableName?: string
	select?: string[]
	conditions?: string
	objectIdentifier?: string
	parent?: string
	skey?: string
	tkey?: string

    public constructor(init?:Partial<NestedTable>) {
        Object.assign(this, init);
    }
}

class TargetTable {
	tableName: string
	columnMappings: ColumnMapper[]

    public constructor(init?:Partial<TargetTable>) {
        Object.assign(this, init);
    }
}

class ColumnMapper {
	source: string
	target: string
	dataType: string
	isPrimaryKey: boolean

    public constructor(init?:Partial<ColumnMapper>) {
        Object.assign(this, init);
    }
}

class TableSelection {
	name: string
	columns: Column[]

    public constructor(init?:Partial<TableSelection>) {
        Object.assign(this, init);
    }
}

class RequestStatus {
	requestId: number
	count: number
	status: string
	message: string

    public constructor(init?:Partial<RequestStatus>) {
        Object.assign(this, init);
    }
}

export { DbSchemaRequest, DbSchemaResponse, Column, SQLToMongoTemplate, SQLToPostgreTemplate, TemplateSettings, Settings, Table, NestedTable, TargetTable, ColumnMapper, TableSelection, RequestStatus };