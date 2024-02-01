﻿CREATE TABLE [dbo].[MushakGeneration] (
    [MushakGenerationId]                        INT             IDENTITY (1, 1) NOT NULL,
    [OrganizationId]                            INT             NOT NULL,
    [MushakForYear]                             INT             NOT NULL,
    [MushakForMonth]                            INT             NOT NULL,
    [GenerateDate]                              DATETIME        NOT NULL,
    [AmountForVat]                              DECIMAL (18, 2) NULL,
    [AmountForSuppDuty]                         DECIMAL (18, 2) NULL,
    [InterestForDueVat]                         DECIMAL (18, 2) NULL,
    [InterestForDueSuppDuty]                    DECIMAL (18, 2) NULL,
    [FinancialPenalty]                          DECIMAL (18, 2) NULL,
    [ExciseDuty]                                DECIMAL (18, 2) NULL,
    [DevelopmentSurcharge]                      DECIMAL (18, 2) NULL,
    [ItDevelopmentSurcharge]                    DECIMAL (18, 2) NULL,
    [HealthDevelopmentSurcharge]                DECIMAL (18, 2) NULL,
    [EnvironmentProtectSurcharge]               DECIMAL (18, 2) NULL,
    [LastClosingVatAmount]                      DECIMAL (18, 2) NULL,
    [LastClosingSuppDutyAmount]                 DECIMAL (18, 2) NULL,
    [PaidVatAmount]                             DECIMAL (18, 2) NULL,
    [VatEconomicCodeId]                         INT             NULL,
    [VatPaymentDate]                            DATETIME        NULL,
    [VatPaymentChallanNo]                       NVARCHAR (20)   NULL,
    [VatPaymentBankBranchId]                    INT             NULL,
    [PaidSuppDutyAmount]                        DECIMAL (18, 2) NULL,
    [SuppDutyEconomicCodeId]                    INT             NULL,
    [SuppDutyPaymentDate]                       DATETIME        NULL,
    [SuppDutyChallanNo]                         NVARCHAR (20)   NULL,
    [SuppDutyBankBranchId]                      INT             NULL,
    [PaidInterestAmountForDueVat]               DECIMAL (18, 2) NULL,
    [InterestForDueVatEconomicCodeId]           INT             NULL,
    [InterestForDueVatPaymentDate]              DATETIME        NULL,
    [InterestForDueVatChallanNo]                NVARCHAR (20)   NULL,
    [InterestForDueVatBankBranchId]             INT             NULL,
    [PaidInterestAmountForDueSuppDuty]          DECIMAL (18, 2) NULL,
    [InterestForDueSuppDutyEconomicCodeId]      INT             NULL,
    [InterestForDueSuppDutyPaymentDate]         DATETIME        NULL,
    [InterestForDueSuppDutyChallanNo]           NVARCHAR (20)   NULL,
    [InterestForDueSuppDutyBankBranchId]        INT             NULL,
    [PaidFinancialPenalty]                      DECIMAL (18, 2) NULL,
    [FinancialPenaltyEconomicCodeId]            INT             NULL,
    [FinancialPenaltyPaymentDate]               DATETIME        NULL,
    [FinancialPenaltyChallanNo]                 NVARCHAR (20)   NULL,
    [FinancialPenaltyBankBranchId]              INT             NULL,
    [PaidExciseDuty]                            DECIMAL (18, 2) NULL,
    [ExciseDutyEconomicCodeId]                  INT             NULL,
    [ExciseDutyPaymentDate]                     DATETIME        NULL,
    [ExciseDutyChallanNo]                       NVARCHAR (20)   NULL,
    [ExciseDutyBankBranchId]                    INT             NULL,
    [PaidDevelopmentSurcharge]                  DECIMAL (18, 2) NULL,
    [DevelopmentSurchargeEconomicCodeId]        INT             NULL,
    [DevelopmentSurchargePaymentDate]           DATETIME        NULL,
    [DevelopmentSurchargeChallanNo]             NVARCHAR (20)   NULL,
    [DevelopmentSurchargeBankBranchId]          INT             NULL,
    [PaidItDevelopmentSurcharge]                DECIMAL (18, 2) NULL,
    [ItDevelopmentSurchargeEconomicCodeId]      INT             NULL,
    [ItDevelopmentSurchargePaymentDate]         DATETIME        NULL,
    [ItDevelopmentSurchargeChallanNo]           NVARCHAR (20)   NULL,
    [ItDevelopmentSurchargeBankBranchId]        INT             NULL,
    [PaidHealthDevelopmentSurcharge]            DECIMAL (18, 2) NULL,
    [HealthDevelopmentSurchargeEconomicCodeId]  INT             NULL,
    [HealthDevelopmentSurchargePaymentDate]     DATETIME        NULL,
    [HealthDevelopmentSurchargeChallanNo]       NVARCHAR (20)   NULL,
    [HealthDevelopmentSurchargeBankBranchId]    INT             NULL,
    [PaidEnvironmentProtectSurcharge]           DECIMAL (18, 2) NULL,
    [EnvironmentProtectSurchargeEconomicCodeId] INT             NULL,
    [EnvironmentProtectSurchargePaymentDate]    DATETIME        NULL,
    [EnvironmentProtectSurchargeChallanNo]      NVARCHAR (20)   NULL,
    [EnvironmentProtectSurchargeBankBranchId]   INT             NULL,
    [MiscIncrementalAdjustmentAmount]           DECIMAL (18, 2) NULL,
    [MiscIncrementalAdjustmentDesc]             NVARCHAR (500)  NULL,
    [MiscDecrementalAdjustmentAmount]           DECIMAL (18, 2) NULL,
    [MiscDecrementalAdjustmentDesc]             NVARCHAR (500)  NULL,
    [IsSubmitted]                               BIT             NULL,
    [SubmissionDate]                            DATETIME        NULL,
    [IsWantToGetBackClosingAmount]              BIT             NOT NULL,
    [ReturnAmountFromClosingVat]                DECIMAL (18, 2) NULL,
    [ReturnFromClosingVatChequeBankId]          INT             NULL,
    [ReturnFromClosingVatChequeNo]              NVARCHAR (50)   NULL,
    [ReturnFromClosingVatChequeDate]            DATETIME        NULL,
    [ReturnAmountFromClosingSd]                 DECIMAL (18, 2) NULL,
    [ReturnFromClosingSdChequeBankId]           INT             NULL,
    [ReturnFromClosingSdChequeNo]               NVARCHAR (50)   NULL,
    [ReturnFromClosingSdChequeDate]             DATETIME        NULL,
    [IsActive]                                  BIT             NOT NULL,
    [MushakGenerationStageId]                   TINYINT         NOT NULL,
    CONSTRAINT [PK_MushakGeneration] PRIMARY KEY CLUSTERED ([MushakGenerationId] ASC),
    CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingSdChequeBankId] FOREIGN KEY ([ReturnFromClosingSdChequeBankId]) REFERENCES [dbo].[Bank] ([BankId]),
    CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingVatChequeBankId] FOREIGN KEY ([ReturnFromClosingVatChequeBankId]) REFERENCES [dbo].[Bank] ([BankId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_DevelopmentSurchargeBankBranchId] FOREIGN KEY ([DevelopmentSurchargeBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_EnvironmentProtectSurchargeBankBranchId] FOREIGN KEY ([EnvironmentProtectSurchargeBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_ExciseDutyBankBranchId] FOREIGN KEY ([ExciseDutyBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_FinancialPenaltyBankBranchId] FOREIGN KEY ([FinancialPenaltyBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_HealthDevelopmentSurchargeBankBranchId] FOREIGN KEY ([HealthDevelopmentSurchargeBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueSuppDutyBankBranchId] FOREIGN KEY ([InterestForDueSuppDutyBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueVatBankBranchId] FOREIGN KEY ([InterestForDueVatBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_ItDevelopmentSurchargeBankBranchId] FOREIGN KEY ([ItDevelopmentSurchargeBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_SuppDutyBankBranchId] FOREIGN KEY ([SuppDutyBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_BankBranch_VatPaymentBankBranchId] FOREIGN KEY ([VatPaymentBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_MushakGeneration_MushakGenerationStage] FOREIGN KEY ([MushakGenerationStageId]) REFERENCES [dbo].[MushakGenerationStage] ([MushakGenerationStageId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_DevelopmentSurchargeEconomicCodeId] FOREIGN KEY ([DevelopmentSurchargeEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_EnvironmentProtectSurchargeEconomicCodeId] FOREIGN KEY ([EnvironmentProtectSurchargeEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ExciseDutyEconomicCodeId] FOREIGN KEY ([ExciseDutyEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_FinancialPenaltyEconomicCodeId] FOREIGN KEY ([FinancialPenaltyEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_HealthDevelopmentSurchargeEconomicCodeId] FOREIGN KEY ([HealthDevelopmentSurchargeEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueSuppDutyEconomicCodeId] FOREIGN KEY ([InterestForDueSuppDutyEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueVatEconomicCodeId] FOREIGN KEY ([InterestForDueVatEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ItDevelopmentSurchargeEconomicCodeId] FOREIGN KEY ([ItDevelopmentSurchargeEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_SuppDutyEconomicCodeId] FOREIGN KEY ([SuppDutyEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_VatEconomicCodeId] FOREIGN KEY ([VatEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_MushakGeneration_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);
