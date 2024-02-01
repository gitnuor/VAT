CREATE TABLE [dbo].[AuditOperation] (
    [AuditOperationID] INT            NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Remarks]          NVARCHAR (500) NULL,
    [IsActive]         BIT            NOT NULL,
    [CreatedBy]        INT            NULL,
    [CreatedTime]      DATETIME       NULL,
    CONSTRAINT [PK_AuditOperation] PRIMARY KEY CLUSTERED ([AuditOperationID] ASC)
);

