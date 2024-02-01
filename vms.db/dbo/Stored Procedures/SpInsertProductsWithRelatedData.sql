CREATE PROCEDURE [dbo].[SpInsertProductsWithRelatedData]
@OrganizationId INT,
@ProductsDetailsJson NVARCHAR(MAX),
@CreateBy INT=0,
@CreatedTime DATETIME=null
AS
BEGIN 

DECLARE @ProductDetails TABLE
(
[Name] [nvarchar](50) NOT NULL,
[ModelNo] [nvarchar](50) NULL,
[Code] [nvarchar](50) NULL,
[HSCode] [nvarchar](50) NULL,
[ProductCategoryId] [int] NULL,
[ProductCategoryName] varchar(50),
[ProductGroupId] [int] NOT NULL,
[ProductGroupsName] varchar(50),
[TotalQuantity] [decimal](18, 2) NOT NULL,
[MeasurementUnitId] [int] NOT NULL,
[MeasurementUnitName] varchar(50),
[EffectiveFrom] [datetime] NOT NULL,
[EffectiveTo] [datetime] NULL,
[IsSellable] [bit] NOT NULL,
[IsRawMaterial] [bit] NOT NULL,
[IsNonRebateable] [bit] NULL,
[ReferenceKey] [nvarchar](100) NULL,
[ModifyDate] [datetime2](7) NULL,
[IsActive] [bit] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedTime] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedTime] [datetime] NULL,
[ApiTransactionId] [bigint] NULL
);

INSERT INTO @ProductDetails
(
[Name],
[ModelNo],
[Code],
[HSCode],
[ProductCategoryId],
[ProductCategoryName],
[ProductGroupId],
[ProductGroupsName],
[TotalQuantity],
[MeasurementUnitId],
[MeasurementUnitName],
[EffectiveFrom],
[EffectiveTo],
[IsSellable],
[IsRawMaterial],
[IsNonRebateable],
[ReferenceKey],
[ModifyDate],
[IsActive],
[CreatedBy],
[CreatedTime],
[ModifiedBy],
[ModifiedTime]
)
SELECT 
jd.[Name],
jd.[ModelNo],
jd.[Code],
jd.[HSCode],
jd.[ProductCategoryId],
jd.[ProductCategoryName],
jd.[ProductGroupId],
jd.[ProductGroupsName],
jd.[TotalQuantity],
jd.[MeasurementUnitId],
jd.[MeasurementUnitName],
jd.[EffectiveFrom],
jd.[EffectiveTo],
jd.[IsSellable],
jd.[IsRawMaterial],
jd.[IsNonRebateable],
jd.[ProductId],
jd.[ModifyDate],
jd.[IsActive],
jd.[CreatedBy],
jd.[CreatedTime],
jd.[ModifiedBy],
jd.[ModifiedTime]

FROM OPENJSON(@ProductsDetailsJson)
WITH
(
[ProductId] [int],
[Name] [nvarchar](50),
[ModelNo] [nvarchar](50),
[Code] [nvarchar](50),
[HSCode] [nvarchar](50),
[ProductCategoryId] [int],
[ProductCategoryName] varchar(50),
[ProductGroupId] [int],
[ProductGroupsName] varchar(50),
[TotalQuantity] [decimal](18, 2),
[MeasurementUnitId] [int],
[MeasurementUnitName] varchar(50),
[EffectiveFrom] [datetime],
[EffectiveTo] [datetime],
[IsSellable] [bit],
[IsRawMaterial] [bit],
[IsNonRebateable] [bit],
[ReferenceKey] [nvarchar](100),
[ModifyDate] [datetime2](7),
[IsActive] [bit],
[CreatedBy] [int],
[CreatedTime] [datetime],
[ModifiedBy] [int],
[ModifiedTime] [datetime]
) jd



DECLARE  @ProductGroups TABLE
(
[OrganizationId] [int] NOT NULL,
[Name] [nvarchar](50) NOT NULL,
[ParentGroupId] [int] NULL,
[Node] [nvarchar](50) NULL,
[IsActive] [bit] NOT NULL,
[ReferenceKey] [nvarchar](100) NULL,
[CreatedBy] [int] NULL,
[CreatedTime] [datetime] NULL
);


insert into ProductGroups
(
OrganizationId,
Name,
ParentGroupId,
Node,
IsActive,
ReferenceKey,
CreatedBy,
CreatedTime
)

select 
distinct
[OrganizationId]= @OrganizationId,
[Name] = t.ProductGroupsName
,[ParentGroupId] = null
,[Node] = null
,[IsActive] = 1
,[ReferenceKey] = t.ProductGroupId
,[CreatedBy] = @CreateBy
,[CreatedTime] = @CreatedTime
from @ProductDetails t
where t.IsActive = 1
--and t.ProjectSetupId = 1
and t.ProductGroupId not in
(
select ReferenceKey from ProductGroups where OrganizationId = @OrganizationId and IsActive = 1
)

insert into ProductCategory 
(
OrganizationId,
Name,
IsActive,
ReferenceKey,
CreatedBy,
CreatedTime,
ModifiedBy,
ModifiedTime,
ApiTransactionId
)
select 
distinct
[OrganizationId] = @OrganizationId
,[Name] = t.ProductCategoryName
,[IsActive] = 1
,[ReferenceKey] = t.ProductCategoryId
,[CreatedBy] = @CreateBy
,[CreatedTime] = @CreatedTime
,[ModifiedBy] = NULL
,[ModifiedTime]=NULL
,[ApiTransactionId] = NULL
from @ProductDetails t
where t.IsActive = 1
and t.ProductCategoryId not in
(
	select ReferenceKey from ProductCategory where OrganizationId = @OrganizationId and IsActive = 1
)


insert into MeasurementUnits 
(
Name,
OrganizationId,
IsActive,
ReferenceKey,
CreatedBy,
CreatedTime,
ModifiedBy,
ModifiedTime,
ApiTransactionId
)
select 
distinct
[Name] = t.MeasurementUnitName
,[OrganizationId]=@OrganizationId
,[IsActive] = 1
,[ReferenceKey]  = t.MeasurementUnitId
,[CreatedBy] = @CreateBy
,[CreatedTime] = @CreatedTime
,[ModifiedBy] = NULL
,[ModifiedTime] = NULL
,[ApiTransactionId] = NULL
from @ProductDetails t
where t.IsActive = 1
and t.MeasurementUnitId not in 
(
	select ReferenceKey from MeasurementUnits  where OrganizationId = @OrganizationId and IsActive = 1
)

INSERT INTO [dbo].[Products]
           ([Name]
           ,[ModelNo]
           ,[Code]
           ,[HSCode]
           ,[ProductCategoryId]
           ,[ProductGroupId]
           ,[OrganizationId]
           ,[TotalQuantity]
           ,[MeasurementUnitId]
           ,[EffectiveFrom]
           ,[EffectiveTo]
           ,[IsSellable]
           ,[IsRawMaterial]
           ,[IsNonRebateable]
           ,[ReferenceKey]
           ,[ModifyDate]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedTime]
           ,[ApiTransactionId])
select 
[Name] = t.Name
,[ModelNo] = null
,[Code] = t.Code
,[HSCode] = null
,[ProductCategoryId] = pc.ProductCategoryId
,[ProductGroupId] = pg.ProductGroupId
,[OrganizationId] = @OrganizationId
,[TotalQuantity] = t.TotalQuantity
,[MeasurementUnitId] = mu.MeasurementUnitId
,[EffectiveFrom] = GETDATE()
,[EffectiveTo] = null
,[IsSellable] = (case when t.IsSellable = '' then 0 else t.IsSellable end)
,[IsRawMaterial] = (case when t.IsRawMaterial = '' then 0 else t.IsRawMaterial end)
,[IsNonRebateable] = (case when t.IsNonRebateable = '' then null else t.IsNonRebateable end)
,[ReferenceKey] = t.ReferenceKey
,[ModifyDate] = GETDATE()
,[IsActive] = 1 
,[CreatedBy] = @CreateBy
,[CreatedTime] = GETDATE()
,[ModifiedBy] = NULL
,[ModifiedTime] = NULL
,[ApiTransactionId] = NULL
from @ProductDetails t 
inner join ProductCategory pc on pc.ReferenceKey = t.ProductCategoryId  
inner join ProductGroups pg on pg.ReferenceKey = t.ProductGroupId 
inner join MeasurementUnits mu on mu.ReferenceKey = t.MeasurementUnitId 
where t.IsActive = 1
and pc.isActive = 1
and pg.IsActive = 1
and mu.IsActive = 1
and pc.OrganizationId = @OrganizationId
and pg.OrganizationId = @OrganizationId
and mu.OrganizationId = @OrganizationId
and t.ReferenceKey not in
(
	select ReferenceKey from Products where OrganizationId = @OrganizationId and IsActive = 1
)


--select * from @ProductDetails
select distinct MeasurementUnitId,MeasurementUnitName from @ProductDetails


--exec SpInsertProductsWithRelatedData 1,''

END
