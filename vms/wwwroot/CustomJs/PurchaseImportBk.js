const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        totalPrice: 'totalPrice',
        productDiscount: 'productDiscount',
        productPriceAfterDiscount: 'productPriceAfterDiscount',
        sdValue: 'sdValue',
        priceForVat: 'priceForVat',
        totalVat: 'totalVat',
        totalPriceWithVat: 'totalPriceWithVat',
        totalPriceWithVatAfterDiscount: 'totalPriceWithVatAfterDiscount'
    },
    payments: [],
    paymentProp: {
        paymentInfoId: 'paymentInfoId',
        paidAmount: 'paidAmount'
    },
    docs: [],
    docProp: {
        documentInfoId: 'documentInfoId'
    }
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
        frmPayment: document.getElementById('frmPayment'),
        frmDocument: document.getElementById('frmDocument')
    },
    mainFormFields: {
        chkIsVatDeductedInSource: document.getElementById('IsVatDeductedInSource'),
        txtVdsAmount: document.getElementById('VdsAmount'),
        drpPurchaseReasonId: document.getElementById('PurchaseReasonId'),
        txtInvoiceNo: document.getElementById('InvoiceNo'),
        datePurchaseDate: document.getElementById('PurchaseDate'),
        drpPurchaseDate: document.getElementById('VendorId'),
        txtVendorInvoiceNo: document.getElementById('VendorInvoiceNo'),
        txtVatChallanNo: document.getElementById('VatChallanNo'),
        dateVatChallanIssueDate: document.getElementById('VatChallanIssueDate')
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        txtQuantity: document.getElementById('Quantity'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        txtDiscountPerItem: document.getElementById('DiscountPerItem'),
        txtTotalProductDiscount: document.getElementById('TotalProductDiscount'),
        txtProductPriceWithDiscount: document.getElementById('ProductPriceWithDiscount'),
        txtSupplementaryDutyPercent: document.getElementById('SupplementaryDutyPercent'),
        txtProductSupplementaryDuty: document.getElementById('ProductSupplementaryDuty'),
        txtVatAblePrice: document.getElementById('VatAblePrice'),
        drpProductVatTypeId: document.getElementById('ProductVatTypeId'),
        txtProductVatPercent: document.getElementById('ProductVatPercent'),
        txtProductVat: document.getElementById('ProductVat'),
        txtProductPriceWithVat: document.getElementById('ProductPriceWithVat'),
        txtProductPriceWithVatAfterDiscount: document.getElementById('ProductPriceWithVatAfterDiscount')
    },
    paymentField: {
        drpPaymentMethodId: document.getElementById('PaymentMethodId'),
        drpBankId: document.getElementById('BankId'),
        txtMobilePaymentWalletNo: document.getElementById('MobilePaymentWalletNo'),
        txtDocumentNoOrTransactionId: document.getElementById('DocumentNoOrTransactionId'),
        txtPaymentDocumentOrTransDate: document.getElementById('PaymentDocumentOrTransDate'),
        txtPaymentDate: document.getElementById('PaymentDate'),
        txtPaidAmount: document.getElementById('PaidAmount'),
        txtPaymentRemarks: document.getElementById('PaymentRemarks')
    },
    documentField: {
        drpDocumentTypeId: document.getElementById('DocumentTypeId'),
        fileUploadedFile: document.getElementById('UploadedFile'),
        txtDocumentRemarks: document.getElementById('DocumentRemarks')
    },
    totalCalculatedTableCell: {
        productTotalPrice: document.getElementById('productTotalPrice'),
        productTotalDiscount: document.getElementById('productTotalDiscount'),
        productTotalPriceAfterDiscount: document.getElementById('productTotalPriceAfterDiscount'),
        productTotalSd: document.getElementById('productTotalSd'),
        productTotalVatablePrice: document.getElementById('productTotalVatablePrice'),
        productTotalVat: document.getElementById('productTotalVat'),
        productTotalPriceWithVat: document.getElementById('productTotalPriceWithVat'),
        productTotalPriceWithVatAfterDiscount: document.getElementById('productTotalPriceWithVatAfterDiscount')
    },
    totalPaidCalculatedTableCell: {
        paymentTotalPaid: document.getElementById('paymentTotalPaid')
    },
    listTables: {
        product: document.getElementById('productTable'),
        productBody: document.getElementById('productTableBody'),
        payment: document.getElementById('paymentTable'),
        paymentBody: document.getElementById('paymentTableBody'),
        doc: document.getElementById('docTable'),
        docBody: document.getElementById('docTableBody')
    },
    btn: {
        btnAddProduct: document.getElementById('btnAddProduct'),
        btnResetProduct: document.getElementById('btnResetProduct'),
        btnAddPayment: document.getElementById('btnAddPayment'),
        btnAddDocument: document.getElementById('btnAddDocument'),
        btnSave: document.getElementById('btnSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
    otherBlock: {
        dueAmount: document.getElementById('dueAmount')
    }
}

const classesToShowHideInEvents = {
    bankPayment: 'payment-bank',
    mobilePayment: 'payment-mobile'
}
classesToShowHideInEvents.paymentRelatedList =
    [classesToShowHideInEvents.bankPayment, classesToShowHideInEvents.mobilePayment];
const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    displayPaymentBank: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.bankPayment);
    },
    displayPaymentMobile: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.mobilePayment);
    },
}
const mainFormFields = {
    frmProductInformation: document.getElementById('frmProductInformation'),
    drpPurchaseReasonId: document.getElementById('PurchaseReasonId'),
    drpVendorId: document.getElementById('VendorId'),
    txtVendorInvoiceNo: document.getElementById('VendorInvoiceNo'),
    txtDiscountTotal: document.getElementById('DiscountTotal'),
    txtLcNo: document.getElementById('LcNo'),
    dateLcDate: document.getElementById('LcDate'),
    txtBillEntry: document.getElementById('BillEntry'),
    dateBoeDate: document.getElementById('BoeDate'),
    dateDueDate: document.getElementById('DueDate'),
    txtLcTerm: document.getElementById('LcTerm'),
    txtPoNo: document.getElementById('PoNo'),
    dateAtpDate: document.getElementById('AtpDate'),
    drpVatCommissionarate: document.getElementById('VatCommissionarate'),
    drpAtpBankBranch: document.getElementById('AtpBankBranch'),
    txtBranch: document.getElementById('Branch'),
    hdnEconomicCodeId: document.getElementById('EconomicCodeId'),
    txtAtpChallanNo: document.getElementById('AtpChallanNo'),
    txtAdvTaxPaidAmount: document.getElementById('AdvTaxPaidAmount'),
  
}

const productInformationField = {
    drpProductId: document.getElementById('ProductId'),
    txtQuantity: document.getElementById('Quantity'),
    txtAv: document.getElementById('Av'),
    txtDiscountTotal: document.getElementById('DiscountTotal'),
    drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
    txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
    txtUnitPrice: document.getElementById('UnitPrice'),
    txtSupplementaryDutyPercent: document.getElementById('SupplementaryDutyPercent'),
    txtSupplementaryDuty: document.getElementById('SupplementaryDuty'),
    txtCustomDutyPercent: document.getElementById('CustomDutyPercent'),
    txtCustomDuty: document.getElementById('CustomDuty'),
    txtImportDutyPercent: document.getElementById('ImportDutyPercent'),
    txtImportDuty: document.getElementById('ImportDuty'),
    txtRegulatoryDutyPercent: document.getElementById('RegulatoryDutyPercent'),
    txtRegulatoryDuty: document.getElementById('RegulatoryDuty'),
    txtAdvanceTaxPercent: document.getElementById('AdvanceTaxPercent'),
    txtAdvanceTax: document.getElementById('AdvanceTax'),
    txtAdvanceIncomeTaxPercent: document.getElementById('AdvanceIncomeTaxPercent'),
    txtAdvanceIncomeTax: document.getElementById('AdvanceIncomeTax'),
    drpProductVatTypeId: document.getElementById('ProductVatTypeId'),
    txtVatpercent: document.getElementById('Vatpercent'),
    txtVat: document.getElementById('Vat'),
    ProductInformationList: [],
    tbdProduct: document.getElementById('tbdProduct'),
    
}

const paymentMethodAddControl = {
    drpPaymentMethodId: document.getElementById('PaymentMethodId'),
    drpBankId: document.getElementById('bankId'),
    paidAmount: document.getElementById('paidAmount'),
    paymentDate: document.getElementById('paymentDate'),
    tbdPayment: document.getElementById('tbdPayment'),
    txtMobilePaymentWalletNo: document.getElementById('mobilePaymentWalletNo'),
    datePaymentDocumentDate: document.getElementById('paymentDocumentDate'),
    txtPaymentRemarks: document.getElementById('paymentRemarks'),
    txtDocumentNoOrTransactionId: document.getElementById('documentNoOrTransactionId'),
    RelatedPaymentList: []
}


const fileAddControl = {
    fileDocument: document.getElementById('fileDocument'),
    drpFileType: document.getElementById('drpFileType'),
    txtDocumentRemarks: document.getElementById('DocumentRemarks'),
    tbdAttachedFiles: document.getElementById('tbdAttachedFiles'),
    RelatedDocumentList: []
}

const btn = {
    btnAddRelatedFile: document.getElementById('btnAddRelatedFile'),
    btnAddRelatedPayment: document.getElementById('btnAddRelatedPayment'),
    btnAddProduct: document.getElementById('btnAddProduct'),

}

const contentTableField = {
    drpDocumentType: document.getElementsByClassName('DocumentType'),
    fileFileUpload: document.getElementsByClassName('FileUpload'),
}

const dataAttributes = {
    payment: {
        isBankingChannel: 'data-is-banking-channel',
        isMobileTransaction: 'data-is-mobile-transaction'
    },
    product: {
        productModel: 'data-product-model',
        productCode: 'data-product-code',
        productSalesUnitPrice: 'data-product-sales-unit-price',
        vatTypeId: 'data-product-vat-type-id',
        vatPercent: 'data-product-default-vat-percent',
        sdPercent: 'data-product-sd-percent',
        productMaxSaleQty: 'data-product-max-sale-quantity',
        measurementUnitId: 'data-product-measurement-unit-id',
        measurementUnitName: 'data-product-measurement-unit-name'
    },
    vechile: {
        vechileIsRequireRegistration: 'data-Is-Require-Registration'
    }
}

paymentMethodAddControl.drpPaymentMethodId.addEventListener('change', (event) => {
    const isBankingChannel =
        window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isBankingChannel);
    const isMobileTransaction =
        window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isMobileTransaction);
    if (isBankingChannel === 'True') {
        commonChangeEventsForDisplay.displayPaymentBank();
    } else if (isMobileTransaction === 'True') {
        commonChangeEventsForDisplay.displayPaymentMobile();
    } else {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
    }
});

btn.btnAddRelatedPayment.addEventListener('click', () => {
    addRelatedPayments();
});

const bankorMobilePayName = () => {
    const isBankingChannel =
        window.generalUtility.getDataAttributeValue.dropdownSelected(paymentMethodAddControl.drpPaymentMethodId, dataAttributes.payment.isBankingChannel);
    let BankorMobilePayName = '';
    if (isBankingChannel === 'True') {
        BankorMobilePayName = paymentMethodAddControl.drpBankId.options[paymentMethodAddControl.drpBankId.selectedIndex].text;
    }else {
        BankorMobilePayName = paymentMethodAddControl.txtMobilePaymentWalletNo.value;
    }
    return BankorMobilePayName;
}
const addRelatedPayments = () => {
    const PaymentMethodName = paymentMethodAddControl.drpPaymentMethodId.options[paymentMethodAddControl.drpPaymentMethodId.selectedIndex].text;


    const newPaymentMethodRndId = Math.random().toString(36).substring(2);
    const newPaymentMethodInfo = {
        RndId: newPaymentMethodRndId,
        BankId: paymentMethodAddControl.drpBankId.value,
        MobilePaymentWalletNo: paymentMethodAddControl.txtMobilePaymentWalletNo.value,
        DocumentNoOrTransactionId: paymentMethodAddControl.txtDocumentNoOrTransactionId.value,
        PaymentDocumentDate: paymentMethodAddControl.datePaymentDocumentDate.value,
        PaymentMethodId: paymentMethodAddControl.drpPaymentMethodId.value,
        PaidAmount: paymentMethodAddControl.paidAmount.value,
        PaymentDate: paymentMethodAddControl.paymentDate.value,
        PaymentRemarks: paymentMethodAddControl.txtPaymentRemarks.value,
    }
    paymentMethodAddControl.RelatedPaymentList.push(newPaymentMethodInfo);
    const newPaymentMethodDataRow = `
        <tr id="row_${newPaymentMethodRndId}">
            <td>${PaymentMethodName}</td>
            <td>${bankorMobilePayName()}</td>
            <td>${paymentMethodAddControl.txtDocumentNoOrTransactionId.value}</td>
            <td>${paymentMethodAddControl.datePaymentDocumentDate.value}</td>
            <td>${paymentMethodAddControl.paymentDate.value}</td>
            <td>${paymentMethodAddControl.paidAmount.value}</td>
            <td>${paymentMethodAddControl.txtPaymentRemarks.value}</td>
            <td>
 <a onclick="removeRelatedPaymentMethod('${newPaymentMethodRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    paymentMethodAddControl.tbdPayment.insertAdjacentHTML('beforeend', newPaymentMethodDataRow);
}
const removeRelatedPaymentMethod = (newPaymentMethodRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${newPaymentMethodRndId}`);
    fileDisplayRowId.remove();
    paymentMethodAddControl.RelatedPaymentList = paymentMethodAddControl.RelatedPaymentList.filter(f => f.RndId !== newPaymentMethodRndId);
}

btn.btnAddRelatedFile.addEventListener('click', () => {
    if ($("#fileForm").valid()) {
        addRelatedFiles();
    }
});

const isFileTypeValid = () => {
    let result = fileTypeValidator.isFileTypeValid();
    result = fileTypeValidator.isFileUploadValid();
    return fileTypeValidator.isFileTypeValid()
        && fileTypeValidator.isFileUploadValid();
}


const vatValidator = {
    required: (ctrl) => {
        if (ctrl.value.length === 0) {
            ctrl.classList.add('vat-error');
            return false;
        }
        ctrl.classList.remove('vat-error');
        return true;
    },
    dropDownRequired: (ctrl) => {
        if (ctrl.selectedIndex === 0) {
            ctrl.classList.add('vat-error');
            return false;
        }
        ctrl.classList.remove('vat-error');
        return true;
    }
}

const fileTypeValidator = {
    isFileTypeValid: () => {
        if (vatValidator.required(fileAddControl.drpFileType)) {
            fileUploadValidationMessage.spnFileType.textContent = "";
            return true;
        }
        else {
            fileUploadValidationMessage.spnFileType.textContent = "File type is required";
        }
        return false;
    },
    isFileUploadValid: () => {
        if (vatValidator.required(fileAddControl.fileDocument)) {
            fileUploadValidationMessage.spnFileDocument.textContent = "";
            return true;
        }
        else {
            fileUploadValidationMessage.spnFileDocument.textContent = "No file added";

        }
        return false;
    }
}

fileAddControl.drpFileType.addEventListener('input', () => {
    fileTypeValidator.isFileTypeValid();
});


const addRelatedFiles = () => {
    const newFiles = fileAddControl.fileDocument.files;
    for (let i = 0; i < newFiles.length; i++) {
        const newFileName = newFiles[i].name;
        const newFileDocumentTypeName = fileAddControl.drpFileType.options[fileAddControl.drpFileType.selectedIndex].text;
        const newFileRndId = Math.random().toString(36).substring(2);
        const newDocumentInfo = {
            RndId: newFileRndId,
            DocumentTypeId: fileAddControl.drpFileType.value,
            DocumentRemarks: fileAddControl.txtDocumentRemarks.value,
            DocumentTypeName: newFileDocumentTypeName,
            UploadedFile: newFiles[i]
        }
        fileAddControl.RelatedDocumentList.push(newDocumentInfo);
        const newFileDataRow = `
        <tr id="row_${newFileRndId}">
            <td>${newFileDocumentTypeName}</td>
            <td>${newFileName}</td>   
            <td>${fileAddControl.txtDocumentRemarks.value}</td>
  <td><a onclick="removeRelatedFile('${newFileRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
        fileAddControl.tbdAttachedFiles.insertAdjacentHTML('beforeend', newFileDataRow);
    }
}

const removeRelatedFile = (fileRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${fileRndId}`);
    fileDisplayRowId.remove();
    fileAddControl.RelatedDocumentList = fileAddControl.RelatedDocumentList.filter(f => f.RndId !== fileRndId);
}
btn.btnAddProduct.addEventListener('click', () => {
    if (!isProductExit(productInformationField.drpProductId.value)) {
        if (window.$("#frmProductInformation").valid())
            addRelatedProducts();
    } else {
        toastr.error('Product Already Exist');
    }
});


//Add Product List


const addRelatedProducts = () => {

    const newProductRndId = Math.random().toString(36).substring(2);
    const newProductInfo = {
        RndId: newProductRndId,
        ProductId: productInformationField.drpProductId.value,
        Quantity: productInformationField.txtQuantity.value,
        Av: productInformationField.txtAv.value,
        MeasurementUnitId: productInformationField.drpMeasurementUnitId.value,
        MeasurementUnitName: productInformationField.txtMeasurementUnitName.value,
        UnitPrice: productInformationField.txtUnitPrice.value,
        SupplementaryDutyPercent: productInformationField.txtSupplementaryDutyPercent.value,
        CustomDutyPercent: productInformationField.txtCustomDutyPercent.value,
        ImportDutyPercent: productInformationField.txtImportDutyPercent.value,
        RegulatoryDutyPercent: productInformationField.txtRegulatoryDutyPercent.value,
        AdvanceTaxPercent: productInformationField.txtAdvanceTaxPercent.value,
        AdvanceIncomeTaxPercent: productInformationField.txtAdvanceIncomeTaxPercent.value,
        ProductVatTypeId: productInformationField.drpProductVatTypeId.value,
        Vatpercent: productInformationField.txtVatpercent.value,
        Vat: productInformationField.txtVat.value,
        ProductName: productInformationField.drpProductId.options[productInformationField.drpProductId.selectedIndex].text,
        VattypeName: productInformationField.drpProductVatTypeId.options[productInformationField.drpProductVatTypeId.selectedIndex].text
    }
    productInformationField.ProductInformationList.push(newProductInfo);
    const newProductDataRow = `
        <tr id="row_${newProductRndId}">
        <td>${newProductInfo.ProductName}</td>
        <td>${newProductInfo.Quantity}</td>
        <td>${newProductInfo.Av}</td>
        <td>${newProductInfo.MeasurementUnitName}</td>
        <td>${newProductInfo.UnitPrice}</td>
        <td>${newProductInfo.SupplementaryDutyPercent}</td>
        <td>${newProductInfo.CustomDutyPercent}</td>
        <td>${newProductInfo.ImportDutyPercent}</td>
        <td>${newProductInfo.RegulatoryDutyPercent}</td>
        <td>${newProductInfo.AdvanceTaxPercent}</td>
        <td>${newProductInfo.AdvanceIncomeTaxPercent}</td>
        <td>${newProductInfo.VattypeName}</td>
        <td>${newProductInfo.Vatpercent}</td>
         <td>${newProductInfo.Vat}</td>
        <td></td>
  <td><a onclick="removeRelatedProduct('${newProductRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    productInformationField.tbdProduct.insertAdjacentHTML('beforeend', newProductDataRow);
}

const removeRelatedProduct = (productRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${productRndId}`);
    fileDisplayRowId.remove();
    productInformationField.ProductInformationList = productInformationField.ProductInformationList.filter(f => f.RndId !== productRndId);
}

const isProductExit = (productId) => {
    if (productInformationField.ProductInformationList.find(f => f.ProductId == productId)) {
        return true;
    }
    return false;
}
//End  Product List


//saveData Request Start
const saveData = (data) => {
    console.log('ttt');
    console.log(data);
    //disContentById('loaderAnimation');
    $.ajax({
        url: 'PurchaseImport',
        data: data,
        cache: false,
        method: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (d) {
            window.location.href = `Details/${d.id}`;
        },
        error: function () {
            // viewErrorFeedBack();
            //hideContentById('loaderAnimation');
        }
    });
}
//saveData Request End


//Start Make Form Data to Object
const sendToPurchaseImportController = () => {
    let formData = new FormData();
    let relatedDocIndex = 0;
    let relatedProductIndex = 0;
    let relatedPaymentIndex = 0;
    formData.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
    formData.append("PurchaseReasonId", mainFormFields.drpPurchaseReasonId.value);
    formData.append("VendorId", mainFormFields.drpVendorId.value);
    formData.append("VendorInvoiceNo", mainFormFields.txtVendorInvoiceNo.value);
    formData.append("LcNo", mainFormFields.txtLcNo.value);
    formData.append("LcDate", mainFormFields.dateLcDate.value);
    formData.append("BillEntry", mainFormFields.txtBillEntry.value);
    formData.append("BoeDate", mainFormFields.dateBoeDate.value);
    formData.append("DueDate", mainFormFields.dateDueDate.value);
    formData.append("LcTerm", mainFormFields.txtLcTerm.value);
    formData.append("PoNo", mainFormFields.txtPoNo.value);
    formData.append("AtpDate", mainFormFields.dateAtpDate.value);
    formData.append("VatCommissionarate", mainFormFields.drpVatCommissionarate.value);
    formData.append("AtpBankBranch", mainFormFields.drpAtpBankBranch.value);
    formData.append("Branch", mainFormFields.txtBranch.value);
    formData.append("hdnEconomicCodeId", mainFormFields.hdnEconomicCodeId.value);
    formData.append("AtpChallanNo", mainFormFields.txtAtpChallanNo.value);
    formData.append("AdvTaxPaidAmount", mainFormFields.txtAdvTaxPaidAmount.value);
   

    for (const relatedDoc of fileAddControl.RelatedDocumentList) {
        formData.append(`VmPurchaseImportDocuments[${relatedDocIndex}].DocumentType`, relatedDoc.DocumentTypeId);
        formData.append(`VmPurchaseImportDocuments[${relatedDocIndex}].FileUpload`, relatedDoc.UploadedFile);
        formData.append(`VmPurchaseImportDocuments[${relatedDocIndex}].DocumentRemarks`, relatedDoc.DocumentRemarks);
        relatedDocIndex++;
    }

    for (const relatedPro of productInformationField.ProductInformationList) {
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].ProductId`, relatedPro.ProductId);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].Quantity`, relatedPro.Quantity);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].Av`, relatedPro.Av);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].MeasurementUnitId`, relatedPro.MeasurementUnitId);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].UnitPrice`, relatedPro.UnitPrice);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].SupplementaryDutyPercent`, relatedPro.SupplementaryDutyPercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].CustomDutyPercent`, relatedPro.CustomDutyPercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].RegulatoryDutyPercent`, relatedPro.RegulatoryDutyPercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].AdvanceTaxPercent`, relatedPro.AdvanceTaxPercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].AdvanceIncomeTaxPercent`, relatedPro.AdvanceIncomeTaxPercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].ProductVatTypeId`, relatedPro.ProductVatTypeId);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].Vatpercent`, relatedPro.Vatpercent);
        formData.append(`VmPurchaseImportDetails[${relatedProductIndex}].Vat`, relatedPro.Vat);
        relatedProductIndex++;
    }

    for (const relatedPay of paymentMethodAddControl.RelatedPaymentList) {
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].PaymentMethodId`, relatedPay.PaymentMethodId);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].BankId`, relatedPay.BankId);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].PaidAmount`, relatedPay.PaidAmount);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].PaymentDate`, relatedPay.PaymentDate);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].MobilePaymentWalletNo`, relatedPay.MobilePaymentWalletNo);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].PaymentDocumentDate`, relatedPay.PaymentDocumentDate);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].PaymentRemarks`, relatedPay.PaymentRemarks);
        formData.append(`VmPurchaseImportPayments[${relatedPaymentIndex}].DocumentNoOrTransactionId`, relatedPay.DocumentNoOrTransactionId);
        relatedPaymentIndex++;
    }
    saveData(formData);
}

//Start Make Form Data to Object