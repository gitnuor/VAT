CREATE TABLE [dbo].[MushakGenerationStage] (
    [MushakGenerationStageId] TINYINT        NOT NULL,
    [Name]                    VARCHAR (100)  NOT NULL,
    [NameInBangla]            NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_MushakGenerationStage] PRIMARY KEY CLUSTERED ([MushakGenerationStageId] ASC)
);

