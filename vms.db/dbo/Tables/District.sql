CREATE TABLE [dbo].[District] (
    [DistrictId]   INT            NOT NULL,
    [Name]         VARCHAR (200)  NOT NULL,
    [NameInBangla] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED ([DistrictId] ASC)
);

