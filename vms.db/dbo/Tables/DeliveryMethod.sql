﻿CREATE TABLE [dbo].[DeliveryMethod] (
    [DeliveryMethodId] INT           NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DeliveryMethod] PRIMARY KEY CLUSTERED ([DeliveryMethodId] ASC)
);

