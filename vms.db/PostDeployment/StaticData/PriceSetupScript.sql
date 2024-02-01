SET IDENTITY_INSERT [dbo].[PriceSetup] ON 

INSERT [dbo].[PriceSetup] ([PriceSetupId], [OrganizationId], [ProductId], [MeasurementUnitId], [BaseTP], [MRP], [PurchaseUnitPrice], [SalesUnitPrice], [EffectiveFrom], [EffectiveTo], [IsActive], [CreatedBy], [CreatedTime]) VALUES (2, 5, 1, 1026, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(N'2019-11-30T16:52:06.270' AS DateTime), NULL, 1, 7, CAST(N'2019-11-30T16:52:06.270' AS DateTime))
INSERT [dbo].[PriceSetup] ([PriceSetupId], [OrganizationId], [ProductId], [MeasurementUnitId], [BaseTP], [MRP], [PurchaseUnitPrice], [SalesUnitPrice], [EffectiveFrom], [EffectiveTo], [IsActive], [CreatedBy], [CreatedTime]) VALUES (3, 5, 2, 1026, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(N'2019-11-30T17:19:58.467' AS DateTime), NULL, 1, 7, CAST(N'2019-11-30T17:19:58.467' AS DateTime))
SET IDENTITY_INSERT [dbo].[PriceSetup] OFF
SET IDENTITY_INSERT [dbo].[PriceSetupProductCost] ON 

INSERT [dbo].[PriceSetupProductCost] ([PriceSetupProductCostId], [PriceSetupId], [RawMaterialId], [RequiredQty], [MeasurementUnitId], [OverHeadCostId], [Cost], [WastagePercentage]) VALUES (2, 2, 18, CAST(10.00 AS Decimal(18, 2)), 1025, NULL, CAST(1000.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)))
INSERT [dbo].[PriceSetupProductCost] ([PriceSetupProductCostId], [PriceSetupId], [RawMaterialId], [RequiredQty], [MeasurementUnitId], [OverHeadCostId], [Cost], [WastagePercentage]) VALUES (3, 3, 18, CAST(30.00 AS Decimal(18, 2)), 1025, NULL, CAST(3000.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[PriceSetupProductCost] OFF
