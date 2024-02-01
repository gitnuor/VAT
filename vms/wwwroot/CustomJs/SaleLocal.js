//All List of Array and Props Name
/// <reference path="site.js" />

const genUtil = window.generalUtility;
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

//Assign Form Related id here Like btn,Field,Table id,Table Body Id,etc
const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
        frmPayment: document.getElementById('frmPayment'),
        frmDocument: document.getElementById('frmDocument')
    },
    mainFormFields: {
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        chkIsVatDeductedInSource: document.getElementById('IsVatDeductedInSource'),
        txtVdsAmount: document.getElementById('VdsAmount'),
        drpCustomerId: document.getElementById('CustomerId'),
        txtCustomerPhoneNumber: document.getElementById('CustomerPhoneNumber'),
        txtWorkOrderNo: document.getElementById('WorkOrderNo'),
        drpSalesDeliveryTypeId: document.getElementById('SalesDeliveryTypeId'),
        dateDeliveryDate: document.getElementById('DeliveryDate'),
        txtReceiverName: document.getElementById('ReceiverName'),
        txtReceiverContactNo: document.getElementById('ReceiverContactNo'),
        txtShippingAddress: document.getElementById('ShippingAddress'),
        txtPONo: document.getElementById('PONo'),
        drpVehicleTypeId: document.getElementById('VehicleTypeId'),
        txtDriverName: document.getElementById('DriverName'),
        txtDriverMobile: document.getElementById('DriverMobile'),
        txtVehicleRegistrationNo: document.getElementById('VehicleRegistrationNo'),
        txtSalesRemarks: document.getElementById('SalesRemarks'),
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtProductDescription: document.getElementById('ProductDescription'),
        txtHscode: document.getElementById('Hscode'),
        txtProductCode: document.getElementById('ProductCode'),
        txtSKUId: document.getElementById('SKUId'),
        txtSKUNo: document.getElementById('SKUNo'),
        txtGoodsId: document.getElementById('GoodsId'),
        txtCurrentStock: document.getElementById('CurrentStock'),
        txtQuantity: document.getElementById('Quantity'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        txtSupplementaryDutyPercent: document.getElementById('SupplementaryDutyPercent'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtProductVatPercent: document.getElementById('ProductVatPercent'),
        drpProductVatTypeId: document.getElementById('ProductVatTypeId'),
        txtDiscountPerItem: document.getElementById('DiscountPerItem'),
        txtProductDiscountPercent: document.getElementById('ProductDiscountPercent'),
        txtTotalProductDiscount: document.getElementById('TotalProductDiscount'),
        txtProductPriceWithDiscount: document.getElementById('ProductPriceWithDiscount'),
        txtProductSupplementaryDuty: document.getElementById('ProductSupplementaryDuty'),
        txtVatAblePrice: document.getElementById('VatAblePrice'),
        txtProductVat: document.getElementById('ProductVat'),
        txtProductPriceWithVat: document.getElementById('ProductPriceWithVat'),
        txtProductPriceWithVatAfterDiscount: document.getElementById('ProductPriceWithVatAfterDiscount'),
    },
    paymentField: {
        drpPaymentMethodId: document.getElementById('PaymentMethodId'),
        drpBankId: document.getElementById('bankId'),
        txtPaymentDate: document.getElementById('paymentDate'),
        txtMobilePaymentWalletNo: document.getElementById('mobilePaymentWalletNo'),
        txtPaymentDocumentOrTransDate: document.getElementById('paymentDocumentOrTransDate'),
        txtPaymentRemarks: document.getElementById('paymentRemarks'),
        txtDocumentNoOrTransactionId: document.getElementById('documentNoOrTransactionId'),
        txtPaidAmount: document.getElementById('paidAmount'),
    },
    documentField: {
        drpDocumentTypeId: document.getElementById('DocumentType'),
        fileUploadedFile: document.getElementById('FileUpload'),
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
        btnFinalSave: document.getElementById('btnFinalSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
    otherBlock: {
        dueAmount: document.getElementById('dueAmount'),
        notificationArea: document.getElementById('notificationArea')
    },
    modal: {
        body: document.getElementById('SalesSummeryModalBody')
    }
}
//Special Item Id
const specialItem = {
    classesToShowHideInEvents: {
        vdsRelated: 'vds-related',
        bankPayment: 'payment-bank',
        mobilePayment: 'payment-mobile',
        vehicleRegistrationNo: 'vehicle-registration-no'
    },
    classesOfDueAmount: [
        window.generallyUsedCssClass.successText, window.generallyUsedCssClass.warningText,
        window.generallyUsedCssClass.dangerText
    ],
    elementToValidate: {
        productChange: [
            elementInformation.productField.drpProductId, elementInformation.productField.drpProductVatTypeId
        ]
    }
}

//Logical boolean Field Assign for Feture Use
const logicalInformation = {
    isVds: false
}

//Payment Hide Show CSS Class List
specialItem.classesToShowHideInEvents.paymentRelatedList =
    [specialItem.classesToShowHideInEvents.bankPayment, specialItem.classesToShowHideInEvents.mobilePayment];

//Vechile Related Field Hide show Css Class
specialItem.classesToShowHideInEvents.vehicleRelatedList =
    [specialItem.classesToShowHideInEvents.vehicleRegistrationNo];

//Price Calculation Common Method
const priceCalculation = {
    calculatePrice: (unitPrice, qty) => {
        return generalUtility.roundNumberOption.byFour(unitPrice * qty);
    },
    calculateUnitPrice: (price, qty) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return generalUtility.roundNumberOption.byFour(price / qty);
    },
    calculateDiscountPerItem: (qty, discount) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return generalUtility.roundNumberOption.byFour(discount / qty);
    },
    calculateDiscount: (qty, discountPerItem) => {
        return generalUtility.roundNumberOption.byFour(qty * discountPerItem);
    },
    calculatePriceAfterDiscount: (price, discount) => {
        return generalUtility.roundNumberOption.byFour(price - discount);
    },
    calculateSdPercent: (price, sd) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return generalUtility.roundNumberOption.byFour(sd * 100 / price);
    },
    calculateSd: (price, sdPercent) => {
        return generalUtility.roundNumberOption.byFour(price * sdPercent / 100);
    },
    calculateVatablePrice: (price, sd) => {
        return generalUtility.roundNumberOption.byFour(price + sd);
    },
    calculateVatPercent: (price, vat) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return generalUtility.roundNumberOption.byFour(vat * 100 / price);
    },
    calculateVat: (vatablePrice, vatPercent) => {
        return generalUtility.roundNumberOption.byFour(vatablePrice * vatPercent / 100);
    },
    calculatePriceWithVatAndSd: (vatablePrice, vat) => {
        return generalUtility.roundNumberOption.byFour(vatablePrice + vat);
    },
    calculatePriceWithVatAndSdAfterDiscount: (priceWithVatAndSd, discount) => {
        return generalUtility.roundNumberOption.byFour(priceWithVatAndSd - discount);
    }
}

//Check Current Stock
const isCurrentStockAvailable = (qty, curStock) => {
    return qty > curStock;
}

//Call Current Stock method to  Check Available Stock
const isStock = (qty, curStock) => {
    //const isInventoryStr = genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId,
    //    dataAttributes.product.isInventory);
    //if (commonChangeEventsForCalculation.isCurStockAvailable(qty, curStock) &&
    //    genUtil.getInputValue.isValueTrue(isInventoryStr)) {
    //    showErrorMessage(validationMessage.msgQuantity, 'Qty not allow Greater then Current Stock');
    //    return false;
    //}
    //showErrorMessage(validationMessage.msgQuantity, '');
    return true;
}

//Call this Method When Need to Show any type of Error Message
const showErrorMessage = (idError, errorMessage) => {
    idError.textContent = errorMessage;
}

//Validation Method for Is Quantity Available
const validationMessage = {
    msgQuantity: document.getElementById('msgQuantity')
}

//Common Change Event to call Calculation Method
const commonChangeEventsForCalculation = {
    removeProducts(msg) {
        if (listInfo.products.length > 0) {
            listInfo.products = [];
            elementInformation.listTables.productBody.innerHTML = '';
            commonUtilitySale.showDueAmount();
            commonUtilitySale.showTotalAmount();
            commonUtilitySale.showTotalPaidAmount();
            commonUtilitySale.fixVdsAmount();
            commonChangeEventsForCalculation.resetForm.product();
            genUtil.message.showErrorMsg(msg);
        }
    },
    removeProductForBranchChange: () => {
        commonChangeEventsForCalculation.removeProducts('All products has been removed due to remove change Branch.');
    },
    removePaymentEvent: () => {
        listInfo.payments = [];
        elementInformation.listTables.paymentBody.innerHTML = '';
        commonUtilitySale.showDueAmount();
        commonUtilitySale.showTotalPaidAmount();
    },
    removeVdsEvent: () => {
        genUtil.displayOption.hideItemByClassName(specialItem.classesToShowHideInEvents.vdsRelated);
        logicalInformation.isVds = false;
        commonChangeEventsForCalculation.removeProducts('All products has been removed due to remove VDS.');
    },
    addVdsEvent: () => {
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.vdsRelated);
        logicalInformation.isVds = true;
        commonUtilitySale.fixVdsAmount();
        commonUtilitySale.showDueAmount();
        commonUtilitySale.showTotalAmount();
        commonUtilitySale.showTotalPaidAmount();
    },
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.txtTotalPrice.value = totalPrice;
        const discount =
            priceCalculation.calculateDiscount(+(elementInformation.productField.txtDiscountPerItem.value), qty);
        elementInformation.productField.txtTotalProductDiscount.value = discount;
        elementInformation.productField.txtProductPriceWithDiscount.value =
            priceCalculation.calculatePriceAfterDiscount(totalPrice, discount);

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, sd);
        elementInformation.productField.txtVatAblePrice.value = vatablePrice;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatablePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, discount);
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +elementInformation.productField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        elementInformation.productField.txtUnitPrice.value = unitPrice;

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);
        elementInformation.productField.txtProductPriceWithDiscount.value =
            priceCalculation.calculatePriceAfterDiscount(totalPrice, totalDiscount);

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, sd);
        elementInformation.productField.txtVatAblePrice.value = vatablePrice;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatablePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount);
    },
    discountPerItemChangeEvent: (discountPerItem) => {
        const qty = +elementInformation.productField.txtQuantity.value;

        const totalDiscount = priceCalculation.calculateDiscount(qty, discountPerItem);
        elementInformation.productField.txtTotalProductDiscount.value = totalDiscount;

        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        elementInformation.productField.txtProductPriceWithDiscount.value =
            priceCalculation.calculatePriceAfterDiscount(totalPrice, totalDiscount);

        const totalPriceWithVat = +elementInformation.productField.txtProductPriceWithVat.value;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            priceCalculation.calculatePriceWithVatAndSdAfterDiscount(totalPriceWithVat, totalDiscount);

    },
    productDiscountChangeEvent: (productDiscount) => {
        const qty = +elementInformation.productField.txtQuantity.value;

        const discountPerItem = priceCalculation.calculateDiscountPerItem(qty, productDiscount);
        elementInformation.productField.txtDiscountPerItem.value = discountPerItem;

        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        elementInformation.productField.txtProductPriceWithDiscount.value =
            priceCalculation.calculatePriceAfterDiscount(totalPrice, productDiscount);

        const totalPriceWithVat = +elementInformation.productField.txtProductPriceWithVat.value;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            priceCalculation.calculatePriceWithVatAndSdAfterDiscount(totalPriceWithVat, productDiscount);

    },
    sdPercentChangeEvent: (sdPercent) => {
        const productPrice = +(elementInformation.productField.txtTotalPrice.value);
        const totalSd = priceCalculation.calculateSd(productPrice, sdPercent);
        elementInformation.productField.txtProductSupplementaryDuty.value = totalSd;

        const vatAblePrice = priceCalculation.calculateVatablePrice(productPrice, totalSd);
        elementInformation.productField.txtVatAblePrice.value = vatAblePrice;
        const vatPercent = +elementInformation.productField.txtProductVatPercent.value;
        const vat = priceCalculation.calculateVat(vatAblePrice, vatPercent);
        elementInformation.productField.txtProductVat.value = vat;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatAblePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat;

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount);
    },
    vatPercentChangeEvent: (vatPercent) => {
        const vatAblePrice = +(elementInformation.productField.txtVatAblePrice.value);

        const vat = priceCalculation.calculateVat(vatAblePrice, vatPercent);
        elementInformation.productField.txtProductVat.value = vat.toFixed(2);

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatAblePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat.toFixed(2);

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount)).toFixed(2);;
    },
    clearPriceEvent: () => {
        elementInformation.productField.txtUnitPrice.value = 0;
        elementInformation.productField.txtQuantity.value = 0;
        elementInformation.productField.txtTotalPrice.value = 0;
        elementInformation.productField.txtDiscountPerItem.value = 0;
        elementInformation.productField.txtTotalProductDiscount.value = 0;
        elementInformation.productField.txtProductPriceWithDiscount.value = 0;
        elementInformation.productField.txtProductSupplementaryDuty.value = 0;
        elementInformation.productField.txtVatAblePrice.value = 0;
        elementInformation.productField.txtProductVat.value = 0;
        elementInformation.productField.txtProductPriceWithVat.value = 0;
        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
            let unitDrpElements = `<option value>Select Unit</option>`;
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
            genUtil.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductVatTypeId);
        },
        payment: () => {
            elementInformation.formInfo.frmPayment.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.paymentField.drpPaymentMethodId);
            genUtil.setDropdownValue.selectPickerReset(elementInformation.paymentField.drpBankId);
        },
        doc: () => {
            elementInformation.formInfo.frmDocument.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.documentField.drpDocumentTypeId);
        }
    },
    notifyForNotifiablePriceChange: (productId, productName) => {
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfRawMaterialWithNotifiableChange/?productId=${productId}`,
            cache: false,
            method: "GET",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend',
                        `<div class="alert alert-warning price-alert" role="alert">Price of element of ${productName
                        } has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declare again.</div>`);
                }
            }
        });
    },
    isCurStockAvailable: (qty, curStock) => {
        return isCurrentStockAvailable(qty, curStock);
    }
}

//Common Show or Hide for Change Event
const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            genUtil.displayOption.hideItemByClassName(element);
        });
    },
    hideVehicleSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.vehicleRelatedList.forEach(element => {
            genUtil.displayOption.hideItemByClassName(element);
        });
    },
    displayPaymentBank: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.bankPayment);
    },
    displayPaymentMobile: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.mobilePayment);
    },
    displayVehicleRegistrationNo: () => {
        commonChangeEventsForDisplay.hideVehicleSpecialOptions();
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.vehicleRegistrationNo);
    },
}

//Common Utility for Sale Calculation Method
const commonUtilitySale = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice);
    },
    getTotalDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.productDiscount);
    },
    getTotalPriceAfterDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.productPriceAfterDiscount);
    },
    getTotalSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.sdValue);
    },
    getTotalVatAblePrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.priceForVat);
    },
    getTotalVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalVat);
    },
    getTotalPriceWithVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPriceWithVat);
    },
    getTotalPriceWithVatAfterDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products,
            listInfo.productProp.totalPriceWithVatAfterDiscount);
    },
    getTotalPaidAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.payments, listInfo.paymentProp.paidAmount);
    },
    removeProduct: (infoId) => {
        listInfo.products =
            generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        commonUtilitySale.showTotalAmount();
        commonUtilitySale.fixVdsAmount();
        commonUtilitySale.showDueAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removePayment: (infoId) => {
        listInfo.payments =
            generalUtility.removeFromObjectArray(listInfo.payments, listInfo.paymentProp.paymentInfoId, infoId);
        commonUtilitySale.showTotalPaidAmount();
        commonUtilitySale.showDueAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removeDoc: (infoId) => {
        listInfo.docs = generalUtility.removeFromObjectArray(listInfo.docs, listInfo.docProp.documentInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.productTotalPrice.innerText = commonUtilitySale.getTotalPrice();
        elementInformation.totalCalculatedTableCell.productTotalDiscount.innerText =
            commonUtilitySale.getTotalDiscount();
        elementInformation.totalCalculatedTableCell.productTotalPriceAfterDiscount.innerText =
            commonUtilitySale.getTotalPriceAfterDiscount();
        elementInformation.totalCalculatedTableCell.productTotalSd.innerText = commonUtilitySale.getTotalSd();
        elementInformation.totalCalculatedTableCell.productTotalVatablePrice.innerText =
            commonUtilitySale.getTotalVatAblePrice();
        elementInformation.totalCalculatedTableCell.productTotalVat.innerText = commonUtilitySale.getTotalVat();
        elementInformation.totalCalculatedTableCell.productTotalPriceWithVat.innerText =
            commonUtilitySale.getTotalPriceWithVat();
        elementInformation.totalCalculatedTableCell.productTotalPriceWithVatAfterDiscount.innerText =
            commonUtilitySale.getTotalPriceWithVatAfterDiscount();
    },
    showTotalPaidAmount: () => {

        elementInformation.totalPaidCalculatedTableCell.paymentTotalPaid.innerText =
            new Intl.NumberFormat('en-IN').format(commonUtilitySale.getTotalPaidAmount());
    },
    showDueAmount: () => {
        const dueBlock = elementInformation.otherBlock.dueAmount;
        let vdsAdjustText = '';
        let vdsAmount = 0;
        if (logicalInformation.isVds) {
            vdsAdjustText = ' (VDS Adjusted)';
            vdsAmount = +elementInformation.mainFormFields.txtVdsAmount.value;
        }
        genUtil.displayOption.removeMultipleCssClass(dueBlock, specialItem.classesOfDueAmount);
        const payableAmount = commonUtilitySale.getTotalPriceWithVatAfterDiscount() - vdsAmount;
        const dueAmount = payableAmount - commonUtilitySale.getTotalPaidAmount();
        if (dueAmount < 0) {
            genUtil.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.dangerText);
        } else if (dueAmount > 0) {
            genUtil.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.warningText);
        } else {
            genUtil.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.successText);
        }

        elementInformation.otherBlock.dueAmount.innerText =
            `Payable Amount: ${payableAmount}${vdsAdjustText}; Due amount: ${dueAmount}`;
        elementInformation.paymentField.txtPaidAmount.value = dueAmount;
    },
    fixVdsAmount: () => {
        if (logicalInformation.isVds) {
            elementInformation.mainFormFields.txtVdsAmount.value = commonUtilitySale.getTotalVat();
        } else {
            elementInformation.mainFormFields.txtVdsAmount.value = '';
        }
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let docIndex = 0;
        let productIndex = 0;
        let paymentIndex = 0;
        listInfo.products.forEach(d => {
            console.log(productIndex);
            formData.append(`Products[${productIndex}].ProductId`, d.productId);
            formData.append(`Products[${productIndex}].ProductDescription`, d.description);
            formData.append(`Products[${productIndex}].Hscode`, d.hsCode);
            formData.append(`Products[${productIndex}].ProductCode`, d.productCode);
            formData.append(`Products[${productIndex}].SKUId`, d.sKUId);
            formData.append(`Products[${productIndex}].SKUNo`, d.sKUNo);
            formData.append(`Products[${productIndex}].GoodsId`, d.goodsId);
            formData.append(`Products[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`Products[${productIndex}].Quantity`, d.quantity);
            formData.append(`Products[${productIndex}].UnitPrice`, d.unitPrice);
            formData.append(`Products[${productIndex}].TotalPrice`, d.totalPrice);
            formData.append(`Products[${productIndex}].DiscountPerItem`, d.discountPerItem);
            formData.append(`Products[${productIndex}].TotalProductDiscount`, d.productDiscount);
            formData.append(`Products[${productIndex}].ProductPriceWithDiscount`, d.productPriceAfterDiscount);
            formData.append(`Products[${productIndex}].SupplementaryDutyPercent`, d.sdPercent);
            formData.append(`Products[${productIndex}].ProductSupplementaryDuty`, d.sdValue);
            formData.append(`Products[${productIndex}].VatAblePrice`, d.priceForVat);
            formData.append(`Products[${productIndex}].ProductVatTypeId`, d.vatTypeId);
            formData.append(`Products[${productIndex}].ProductVatPercent`, d.vatPercent);
            formData.append(`Products[${productIndex}].ProductVat`, d.totalVat);
            formData.append(`Products[${productIndex}].ProductPriceWithVat`, d.totalPriceWithVat);
            formData.append(`Products[${productIndex}].ProductPriceWithVatAfterDiscount`,
                d.totalPriceWithVatAfterDiscount);
            productIndex++;
        });
        listInfo.payments.forEach(d => {
            formData.append(`Payments[${paymentIndex}].PaymentMethodId`, d.paymentMethodId);
            formData.append(`Payments[${paymentIndex}].IsBankingChannel`, d.isBankingChannel);
            formData.append(`Payments[${paymentIndex}].IsMobileTransaction`, d.isMobileTransaction);
            formData.append(`Payments[${paymentIndex}].BankId`, d.bankInfoId);
            formData.append(`Payments[${paymentIndex}].MobilePaymentWalletNo`, d.mobilePaymentWalletNo);
            formData.append(`Payments[${paymentIndex}].PaidAmount`, d.paidAmount);
            formData.append(`Payments[${paymentIndex}].PaymentDate`, d.paymentDate);
            formData.append(`Payments[${paymentIndex}].DocumentNoOrTransactionId`, d.documentNoOrTransactionId);
            formData.append(`Payments[${paymentIndex}].PaymentDocumentOrTransDate`, d.paymentDocumentOrTransDate);
            formData.append(`Payments[${paymentIndex}].PaymentRemarks`, d.paymentRemarks);
            paymentIndex++;
        });
        listInfo.docs.forEach(d => {
            formData.append(`Documents[${docIndex}].DocumentTypeId`, d.documentTypeId);
            formData.append(`Documents[${docIndex}].UploadedFile`, d.uploadedDocument);
            formData.append(`Documents[${docIndex}].DocumentRemarks`, d.documentRemarks);
            docIndex++;
        });
        formData.append(`SalesRemarks`, elementInformation.mainFormFields.txtSalesRemarks.value);
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'SaleLocal',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                window.location.href = `Details/${d.id}`;
            },
            error: function (d) {
                genUtil.message.showErrorMsg(d);
                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnFinalSave, 'disabled');
            }
        });
    },
    postFormDataAsDraft: data => {
        window.$.ajax({
            url: 'SaleLocalDraft',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                window.location.href = `Details/${d.id}`;
            },
            error: function (d) {
                genUtil.message.showErrorMsg(d);
                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnFinalSave, 'disabled');
            }
        });
    },
    getProductsForSale: async (branchId) => {
        const url = `/api/salesproducts/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
    getCustomerByBranch: async (branchId) => {
        const url = `/api/customerbybranch/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

//Data Attribute Id Assign
const dataAttributes = {
    payment: {
        isBankingChannel: 'data-is-banking-channel',
        isMobileTransaction: 'data-is-mobile-transaction'
    },
    product: {
        productModel: 'data-product-model',
        hsCode: 'data-product-hs-code',
        productCode: 'data-product-code',
        productSalesUnitPrice: 'data-product-sales-unit-price',
        vatTypeId: 'data-product-vat-type-id',
        vatPercent: 'data-product-default-vat-percent',
        isVatUpdatable: 'data-is-vat-updatable',
        sdPercent: 'data-product-sd-percent',
        productMaxSaleQty: 'data-product-max-sale-quantity',
        measurementUnitId: 'data-product-measurement-unit-id',
        measurementUnitName: 'data-product-measurement-unit-name',
        isInventory: 'data-is-inventory',
        isMeasurable: 'data-is-measurable'
    },
    vechile: {
        vechileIsRequireRegistration: 'data-Is-Require-Registration'
    },
    customer: {
        bin: 'data-bin',
        recName: 'data-rec-name',
        cusMobile: 'data-cus-mobile',
        address: 'data-address',
        recMobile: 'data-rec-mobile'
    },
    branch: {
        address: 'data-address'
    },
    productMeasurement: {
        productMeasurementConvertionRatio: 'data-product-convertion-ratio'
    },
}

//Call Show Hide Field Depend on PaymentMethod Change
elementInformation.paymentField.drpPaymentMethodId.addEventListener('change',
    (event) => {
        const isBankingChannel =
            genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isBankingChannel);
        const isMobileTransaction =
            genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isMobileTransaction);
        if (genUtil.getInputValue.isValueTrue(isBankingChannel)) {
            commonChangeEventsForDisplay.displayPaymentBank();
        } else if (genUtil.getInputValue.isValueTrue(isMobileTransaction)) {
            commonChangeEventsForDisplay.displayPaymentMobile();
        } else {
            commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        }
    });

//Set Product Related Field Method
const setProductInfoByProductId = (event) => {

    const drp = event.target;
    commonChangeEventsForCalculation.clearPriceEvent();

    const prodDataAttr = dataAttributes.product;
    const prodFld = elementInformation.productField;
    prodFld.txtHscode.value = genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.hsCode);
    prodFld.txtCurrentStock.value = genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.productMaxSaleQty);

    //prodFld.hdnMeasurementUnitId.value =
    //    genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);

    prodFld.txtMeasurementUnitName.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);

    prodFld.txtUnitPrice.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.productSalesUnitPrice);

    genUtil.setDropdownValue.selectPickerByControl(prodFld.drpProductVatTypeId,
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatTypeId));

    prodFld.txtSupplementaryDutyPercent.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.sdPercent);

    const txtVatPercent = prodFld.txtProductVatPercent;
    const txtQty = prodFld.txtQuantity;

    txtVatPercent.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);


    if (genUtil.getInputValue.isValueTrue(
        genUtil.getDataAttributeValue.dropdownSelected(prodFld.drpProductVatTypeId, prodDataAttr.isVatUpdatable))) {
        genUtil.alterAttr.removeAttr(txtVatPercent, genUtil.elAttr.readOnly.name);
    } else {
        genUtil.alterAttr.setAttr(txtVatPercent, genUtil.elAttr.readOnly.name, genUtil.elAttr.readOnly.value);
    }


    if (genUtil.getInputValue.isValueTrue(
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.isMeasurable))) {
        genUtil.alterAttr.removeAttr(txtQty, genUtil.elAttr.readOnly.name);
        txtQty.value = 0;
    } else {
        txtQty.value = 1;
        genUtil.alterAttr.setAttr(txtQty, genUtil.elAttr.readOnly.name, genUtil.elAttr.readOnly.value);
    }
}

const setMeasurementUnitByProduct = (event) => {
    let unitDrpElements = `<option value>Select Unit</option>`;
    const productId = event.target.value;
    if (productId) {

        commonUtilitySale.getMeasurementUnitForProduct(productId).then(products => {
            products.forEach(element => {
                unitDrpElements += `
                    <option value='${element.measurementUnitId}'
                    data-product-convertion-ratio='${element.convertionRatio}'>
                        ${element.measurementUnitName}
                    </option>
                `;
            });
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
        }).then(() => {
            genUtil.setDropdownValue.selectPickerByControl(elementInformation.productField.hdnMeasurementUnitId,
                genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, dataAttributes.product.measurementUnitId));
        });

    } else {
        genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
            unitDrpElements);
    }

}

//Call Set Product Related Field Method
elementInformation.productField.drpProductId.addEventListener('change',
    (event) => {
        setMeasurementUnitByProduct(event);
        setProductInfoByProductId(event);
    });


elementInformation.mainFormFields.drpCustomerId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.customer;
        elementInformation.mainFormFields.txtReceiverContactNo.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recMobile);

        elementInformation.mainFormFields.txtCustomerPhoneNumber.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.cusMobile);

        elementInformation.mainFormFields.txtReceiverName.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recName);

        elementInformation.mainFormFields.txtShippingAddress.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.address);
    });


elementInformation.mainFormFields.drpOrgBranchId.addEventListener('change',
    (event) => {
        let prodDrpElements = `<option value>Select Product</option>`;
        let custDrpElements = `<option value>Select Customer</option>`;
        const branchId = event.target.value;
        if (branchId) {

            commonUtilitySale.getProductsForSale(branchId).then(products => {
                products.forEach(element => {
                    prodDrpElements += `
                    <option value='${element.productId}'
                            data-product-model='${element.modelNo}'
                            data-product-hs-code='${element.hsCode}'
                            data-product-code='${element.productCode}'
                            data-product-sales-unit-price='${element.salesUnitPrice}'
                            data-product-vat-type-id='${element.productVatTypeId}'
                            data-product-default-vat-percent='${element.defaultVatPercent}'
                            data-product-sd-percent='${element.supplementaryDutyPercent}'
                            data-product-max-sale-quantity='${element.maxSaleQty}'
                            data-product-measurement-unit-id='${element.measurementUnitId}'
                            data-product-measurement-unit-name='${element.measurementUnitName}'
                            data-is-inventory='${element.isInventory}'
                            data-is-measurable='${element.isMeasurable}'>
                        ${element.productDescription}
                    </option>
                `;
                });
                genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.drpProductId,
                    prodDrpElements);

            });

            commonUtilitySale.getCustomerByBranch(branchId).then(brnc => {
                brnc.forEach(element => {
                    custDrpElements += `
                    <option value='${element.customerId}'
                            data-address='${element.deliveryAddress}'
                            data-bin='${element.customerBin}'
                            data-rec-mobile='${element.contactPersonMobile}'
                            data-rec-name='${element.contactPerson}'
                            data-cus-mobile='${element.customerContactNo}'>
                        ${element.customerName}
                    </option>
                `;
                });
                genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpCustomerId,
                    custDrpElements);

            });

        } else {
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpCustomerId,
                custDrpElements);
        }
        commonChangeEventsForCalculation.removeProductForBranchChange();
    });


//Unit Price
elementInformation.productField.txtUnitPrice.addEventListener('input',
    (event) => {
        const unitPrice = +event.target.value;
        const qty = +(elementInformation.productField.txtQuantity.value);
        commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
    });

//Quantity
elementInformation.productField.txtQuantity.addEventListener('input',
    (event) => {

        const unitPrice = +(elementInformation.productField.txtUnitPrice.value);
        const qty = +event.target.value;
        const curStock = +(elementInformation.productField.txtCurrentStock.value);
        commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
        isStock(qty, curStock);
    });

//Total Price
elementInformation.productField.txtTotalPrice.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.totalPriceChangeEvent(+event.target.value);
    });


//Discount per Item
elementInformation.productField.txtDiscountPerItem.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.discountPerItemChangeEvent(+event.target.value);
    });

//Product Discount
elementInformation.productField.txtTotalProductDiscount.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.productDiscountChangeEvent(+event.target.value);
    });

//SD Percent
elementInformation.productField.txtSupplementaryDutyPercent.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.sdPercentChangeEvent(+event.target.value);
    });

//VAT Percent
elementInformation.productField.txtProductVatPercent.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.vatPercentChangeEvent(+event.target.value);
    });


//Vat type change event
elementInformation.productField.drpProductVatTypeId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const txtVatPercent = elementInformation.productField.txtProductVatPercent;
        const prodDataAttr = dataAttributes.product;
        txtVatPercent.value = genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);

        if (genUtil.getInputValue.isValueTrue(
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.isVatUpdatable))) {
            genUtil.alterAttr.removeAttr(txtVatPercent, genUtil.elAttr.readOnly.name);
        } else {
            genUtil.alterAttr.setAttr(txtVatPercent, genUtil.elAttr.readOnly.name, genUtil.elAttr.readOnly.value);
        }
        commonChangeEventsForCalculation.vatPercentChangeEvent(txtVatPercent.value);
    });

//Payment method change
elementInformation.paymentField.drpPaymentMethodId.addEventListener('change',
    (event) => {
        const isBankingChannel =
            genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isBankingChannel);
        const isMobileTransaction =
            genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isMobileTransaction);
        if (genUtil.getInputValue.isValueTrue(isBankingChannel)) {
            commonChangeEventsForDisplay.displayPaymentBank();
        } else if (genUtil.getInputValue.isValueTrue(isMobileTransaction)) {
            commonChangeEventsForDisplay.displayPaymentMobile();
        } else {
            commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        }
    });

//Add Product in Cart
const addRelatedProducts = () => {
    const productInfo = {
        productInfoId: genUtil.getRandomString(6),
        productId: +elementInformation.productField.drpProductId.value,
        currentStock: +elementInformation.productField.txtCurrentStock.value,
        productName: genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.productField.drpProductId),
        description: elementInformation.productField.txtProductDescription.value,
        hsCode: elementInformation.productField.txtHscode.value,
        productCode: elementInformation.productField.txtProductCode.value,
        goodsId: elementInformation.productField.txtGoodsId.value,
        sKUId: elementInformation.productField.txtSKUId.value,
        sKUNo: elementInformation.productField.txtSKUNo.value,
        measurementUnitId: +elementInformation.productField.hdnMeasurementUnitId.value,
        measurementUnitName: elementInformation.productField.txtMeasurementUnitName.value,
        unitPrice: +elementInformation.productField.txtUnitPrice.value,
        unitPriceToDisplay: +elementInformation.productField.txtUnitPrice.value,
        quantity: +elementInformation.productField.txtQuantity.value,
        quantityToDisplay: +elementInformation.productField.txtQuantity.value,
        totalPrice: priceCalculation.calculatePrice(+elementInformation.productField.txtUnitPrice.value,
            +elementInformation.productField.txtQuantity.value),
        totalPriceToDisplay: priceCalculation.calculatePrice(+elementInformation.productField.txtUnitPrice.value,
            +elementInformation.productField.txtQuantity.value),
        discountPerItem: +elementInformation.productField.txtDiscountPerItem.value,
        discountPerItemToDisplay: +elementInformation.productField.txtDiscountPerItem.value,
        productDiscount: +elementInformation.productField.txtTotalProductDiscount.value,
        productDiscountToDisplay: +elementInformation.productField.txtTotalProductDiscount.value,
        productPriceAfterDiscount: +elementInformation.productField.txtProductPriceWithDiscount.value,
        productPriceAfterDiscountToDisplay: +elementInformation.productField.txtProductPriceWithDiscount.value,
        sdPercent: +elementInformation.productField.txtSupplementaryDutyPercent.value,
        sdPercentToDisplay: +elementInformation.productField.txtSupplementaryDutyPercent.value,
        sdValue: +elementInformation.productField.txtProductSupplementaryDuty.value,
        sdValueToDisplay: +elementInformation.productField.txtProductSupplementaryDuty.value,
        priceForVat: +elementInformation.productField.txtVatAblePrice.value,
        priceForVatToDisplay: +elementInformation.productField.txtVatAblePrice.value,
        vatTypeId: +elementInformation.productField.drpProductVatTypeId.value,
        vatTypeName: genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.productField
            .drpProductVatTypeId),
        vatPercent: +elementInformation.productField.txtProductVatPercent.value,
        vatPercentToDisplay: +elementInformation.productField.txtProductVatPercent.value,
        totalVat: +elementInformation.productField.txtProductVat.value,
        totalVatToDisplay: +elementInformation.productField.txtProductVat.value,
        totalPriceWithVat: +elementInformation.productField.txtProductPriceWithVat.value,
        totalPriceWithVatToDisplay: +elementInformation.productField.txtProductPriceWithVat.value,
        totalPriceWithVatAfterDiscount: +elementInformation.productField.txtProductPriceWithVatAfterDiscount.value,
        totalPriceWithVatAfterDiscountToDisplay: +elementInformation.productField.txtProductPriceWithVatAfterDiscount
            .value
    }

    listInfo.products.push(productInfo);

    const dataRow = `
            <tr id="tr_${productInfo.productInfoId}">
                <td>
                    ${productInfo.productName}
                </td>
                <td>
                    ${productInfo.sKUNo}
                </td>
                <td>
                    ${productInfo.sKUId}
                </td>
                  <td>
                    ${productInfo.currentStock}
                </td>
                <td>
                    ${productInfo.measurementUnitName}
                </td>
                <td class="text-end">
                    ${productInfo.unitPriceToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.quantityToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.productDiscountToDisplay} (@${productInfo.discountPerItemToDisplay}/item)
                </td>
                <td class="text-end">
                    ${productInfo.productPriceAfterDiscountToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.sdValueToDisplay} (@${productInfo.sdPercentToDisplay}%)
                </td>
                <td class="text-end">
                    ${productInfo.priceForVatToDisplay}
                </td>
                <td>
                    ${productInfo.vatTypeName}
                </td>
                <td class="text-end">
                    ${productInfo.totalVatToDisplay} (@${productInfo.vatPercentToDisplay}%)
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceWithVatToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceWithVatAfterDiscountToDisplay}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilitySale.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
    elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
    commonUtilitySale.showTotalAmount();
    commonUtilitySale.fixVdsAmount();
    commonUtilitySale.showDueAmount();
    commonChangeEventsForCalculation.resetForm.product();
    commonChangeEventsForCalculation.notifyForNotifiablePriceChange(productInfo.productId, productInfo.productName);
}

//Check is Duplicate Product
const isProductExit = (productId) => {
    if (listInfo.products.find(f => f.productId == productId)) {
        return true;
    }
    return false;
}

//Listeners
//VAT deducted at source
elementInformation.mainFormFields.chkIsVatDeductedInSource.addEventListener('change',
    () => {
        if (event.target.checked) {
            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.addVdsEvent();
        } else {
            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.removeVdsEvent();
        }
    });

//VDS Amount
elementInformation.mainFormFields.txtVdsAmount.addEventListener('change',
    () => {
        const vdsAmountTextBox = event.target;
        if (!logicalInformation.isVds) {
            vdsAmountTextBox.value = '';
            return;
        }
        const totalVatAmount = commonUtilitySale.getTotalVat();
        const vdsAmount = +vdsAmountTextBox.value;
        if (vdsAmount > totalVatAmount) {
            genUtil.message.showErrorMsg("VDS amount should be less than or equal to VAT amount!");

        }
    });

//Call Add Payment Method
elementInformation.btn.btnAddPayment.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmPayment)) {
            addRelatedPayments();
        }
    });
//Call Add Product Method Call
elementInformation.btn.btnAddProduct.addEventListener('click',
    () => {
        //if (!isProductExit(elementInformation.productField.drpProductId.value)) {
        const qty = +(elementInformation.productField.txtQuantity.value);
        const curStock = +(elementInformation.productField.txtCurrentStock.value);
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmProductInformation) &&
            isStock(qty, curStock))
            addRelatedProducts();
        //} else {
        //    toastr.error('Product Already Exist');
        //}
    });

const addRelatedPayments = () => {
    const drpBankId = elementInformation.paymentField.drpBankId;
    const drpPaymentMethodId = elementInformation.paymentField.drpPaymentMethodId;
    let bankInfoId = null;
    let bankName = null;
    let paymentDocumentOrTransDate = null;
    let bankNameOrWalletDisplayValue = '';

    const isBankingChannel =
        generalUtility.getInputValue.isValueTrue(
            genUtil.getDataAttributeValue.dropdownSelected(drpPaymentMethodId,
                dataAttributes.payment.isBankingChannel));
    const isMobileTransaction =
        generalUtility.getInputValue.isValueTrue(
            genUtil.getDataAttributeValue.dropdownSelected(drpPaymentMethodId,
                dataAttributes.payment.isMobileTransaction));
    if (isBankingChannel) {
        bankInfoId = drpBankId.value;
        bankName = genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.paymentField
            .drpBankId);
        paymentDocumentOrTransDate = elementInformation.paymentField.txtPaymentDocumentOrTransDate.value;
        bankNameOrWalletDisplayValue = bankName;
    } else if (isMobileTransaction) {
        paymentDocumentOrTransDate = elementInformation.paymentField.txtPaymentDocumentOrTransDate.value;
        bankNameOrWalletDisplayValue = elementInformation.paymentField.txtMobilePaymentWalletNo.value;
    }

    const payment = {
        paymentInfoId: genUtil.getRandomString(6),
        paymentMethodId: +elementInformation.paymentField.drpPaymentMethodId.value,
        paymentMethodName: genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.paymentField
            .drpPaymentMethodId),
        isBankingChannel: isBankingChannel,
        isMobileTransaction: isMobileTransaction,
        bankInfoId: bankInfoId,
        bankName: bankName,
        mobilePaymentWalletNo: elementInformation.paymentField.txtMobilePaymentWalletNo.value,
        documentNoOrTransactionId: elementInformation.paymentField.txtDocumentNoOrTransactionId.value,
        paymentDocumentOrTransDate: paymentDocumentOrTransDate,
        paymentDate: elementInformation.paymentField.txtPaymentDate.value,
        paidAmount: +elementInformation.paymentField.txtPaidAmount.value,
        paidAmountToDisplay: +elementInformation.paymentField.txtPaidAmount.value,
        paymentRemarks: elementInformation.paymentField.txtPaymentRemarks.value
    }

    listInfo.payments.push(payment);

    const dataRow = `
            <tr id="tr_${payment.paymentInfoId}">
                <td>
                    ${payment.paymentMethodName}
                </td>
                <td>
                    ${bankNameOrWalletDisplayValue}
                </td>
                <td>
                    ${payment.documentNoOrTransactionId}
                </td>
                <td>
                    ${payment.paymentDocumentOrTransDate === null ? '' : payment.paymentDocumentOrTransDate}
                </td>
                <td>
                    ${payment.paymentDate}
                </td>
                <td class="text-end">
                    ${payment.paidAmountToDisplay}
                </td>
                <td>
                    ${payment.paymentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilitySale.removePayment('${payment.paymentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
    elementInformation.listTables.paymentBody.insertAdjacentHTML('beforeend', dataRow);
    commonUtilitySale.showTotalPaidAmount();
    commonUtilitySale.showDueAmount();
    //commonUtilitySale.showDueAmount();
    //showTotalPaidAmount
    commonChangeEventsForCalculation.resetForm.payment();
}
//Add Document Local to Cart
const addRelatedFiles = () => {
    const uploadedFiles = elementInformation.documentField.fileUploadedFile.files;
    const docRemarks = elementInformation.documentField.txtDocumentRemarks.value;
    const documentTypeId = elementInformation.documentField.drpDocumentTypeId.value;
    const documentTypeName =
        genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.documentField.drpDocumentTypeId);
    for (let i = 0; i < uploadedFiles.length; i++) {
        const documentInfo = {
            documentInfoId: genUtil.getRandomString(6),
            documentTypeId: documentTypeId,
            documentTypeName: documentTypeName,
            uploadedDocumentName: uploadedFiles[i].name,
            uploadedDocument: uploadedFiles[i],
            documentRemarks: docRemarks
        }

        listInfo.docs.push(documentInfo);
        const dataRow = `
            <tr id="tr_${documentInfo.documentInfoId}">
                <td>
                    ${documentInfo.documentTypeName}
                </td>
                <td>
                    ${documentInfo.uploadedDocumentName}
                </td>
                <td>
                    ${documentInfo.documentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilitySale.removeDoc('${documentInfo
                .documentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
        elementInformation.listTables.docBody.insertAdjacentHTML('beforeend', dataRow);
    }
    commonChangeEventsForCalculation.resetForm.doc();
}

//Add Document Local to Cart Method Call
elementInformation.btn.btnAddDocument.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmDocument)) {
            addRelatedFiles();
        }
    });
//File Type Validator
const isFileTypeValid = () => {
    let result = fileTypeValidator.isFileTypeValid();
    result = fileTypeValidator.isFileUploadValid();
    return fileTypeValidator.isFileTypeValid() && fileTypeValidator.isFileUploadValid();
}

//vat validation Logic
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
//File Type Validator
const fileTypeValidator = {
    isFileTypeValid: () => {
        if (vatValidator.required(elementInformation.documentField.drpDocumentTypeId)) {
            fileUploadValidationMessage.spnFileType.textContent = "";
            return true;
        } else {
            fileUploadValidationMessage.spnFileType.textContent = "File type is required";
        }
        return false;
    },
    isFileUploadValid: () => {
        if (vatValidator.required(elementInformation.documentField.fileUploadedFile)) {
            fileUploadValidationMessage.spnFileDocument.textContent = "";
            return true;
        } else {
            fileUploadValidationMessage.spnFileDocument.textContent = "No file added";

        }
        return false;
    }
}

//Check Document type is Valid
elementInformation.documentField.drpDocumentTypeId.addEventListener('input',
    () => {
        fileTypeValidator.isFileTypeValid();
    });

//Vechile On Change to Show or Hide Vehicle Registration No Field
elementInformation.mainFormFields.drpVehicleTypeId.addEventListener('change',
    (event) => {
        const isVehicleRegistrationNo =
            genUtil.getDataAttributeValue.dropdownSelected(event.target,
                dataAttributes.vechile.vechileIsRequireRegistration);
        if (genUtil.getInputValue.isValueTrue(isVehicleRegistrationNo)) {
            commonChangeEventsForDisplay.displayVehicleRegistrationNo();
        } else {
            elementInformation.mainFormFields.txtVehicleRegistrationNo.value = "";
            commonChangeEventsForDisplay.hideVehicleSpecialOptions();
        }
    });

elementInformation.productField.hdnMeasurementUnitId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        const measurementDataAttr = dataAttributes.productMeasurement;
        //commonChangeEventsForCalculation.clearQuantityEvent();
        const prodFld = elementInformation.productField;
        // change current stock according to measurement unit
        prodFld.txtCurrentStock.value = (
            (+genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, prodDataAttr.productMaxSaleQty))
            *
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        );
        // change current stock according to measurement unit
        prodFld.txtUnitPrice.value = genUtil.roundNumberOption.byNumber((
            (+genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, prodDataAttr.productSalesUnitPrice))
            /
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        ), 8);
    });

//Save Information
elementInformation.btn.btnSave.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                genUtil.message.showErrorMsg("Can not save without product!!!");
                return;
            } else {
                elementInformation.modal.body.innerHTML = getMushakForPreview();
                const saleDraft = new window.bootstrap.Modal(document.getElementById('SalesSummeryModal'),
                    {
                        backdrop: 'static'
                    });
                saleDraft.show();
            }

        }
    });
elementInformation.btn.btnFinalSave.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                genUtil.message.showErrorMsg("Can not save without product!!!");
                return;
            } else {
                generalUtility.alterAttr.setAttr(elementInformation.btn.btnFinalSave, 'disabled', 'disabled');
                commonUtilitySale.postFormData(commonUtilitySale.getFormData());
            }

        }
    });

//Save as a draft information
elementInformation.btn.btnSaveDraft.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                genUtil.message.showErrorMsg("Can not save without product!!!");
                return;
            } else {
                commonUtilitySale.postFormDataAsDraft(commonUtilitySale.getFormData());
            }
        }
    });

//Reset Whole form
elementInformation.btn.btnResetForm.addEventListener('click',
    () => {
        location.reload();
    });


elementInformation.btn.btnResetProduct.addEventListener('click',
    () => {
        commonChangeEventsForCalculation.resetForm.product();
    });

var getMushakForPreview = () => {
    let salesDetailsText = ``;
    let draftDetailSl = 0;
    listInfo.products.forEach(item => {
        draftDetailSl++;
        salesDetailsText += `
                <tr>
                  <td class="serial-cell">${draftDetailSl}</td>
                  <td class="name-cell">${item.productName}</td>
                  <td class="unit-name-cell">${item.measurementUnitName}</td>
                  <td class="amount-quantity-cell">${item.quantityToDisplay}</td>
                  <td class="amount-quantity-cell">${item.unitPriceToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalPriceToDisplay}</td>
                  <td class="amount-quantity-cell">${item.sdPercentToDisplay}</td>
                  <td class="amount-quantity-cell">${item.sdValueToDisplay}</td>
                  <td class="amount-quantity-cell">${item.vatPercentToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalVatToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalPriceWithVatAfterDiscountToDisplay}</td>
                </tr>
`;
    });

    let draftText = `
    <div class="report-section">
      <div class="report-margin">
        <div class="row">
          <div class="col-2"></div>

          <div class="col-8 report-header mt-3">
            <div>GOVERNMENT OF THE PEOPLE&#x27;S REPUBLIC OF BANGLADESH</div>
            <div class="sub-header">NATIONAL BOARD OF REVENUE</div>
          </div>         

          <div class="col-12 report-header">
            <div class="sub-header">Tax Invoice</div>
            <div class="sub-header">
              [Note clause (c) and clause (f) of sub-rule (1) of rule 40]
            </div>
          </div>
          <div class="col-12 text-center mt-2">
            <div>Name of the registered person: ${window.orgInfo.name}</div>
            <div>BIN of the registered person: ${window.orgInfo.bin}</div>
            <div class="company-other-info">
              Address of the registered person: ${window.orgInfo.address}
            </div>
          </div>          
          <div class="col-8 mt-3">
            <div style="margin-top: 3px">Customer Name: ${elementInformation.mainFormFields.drpCustomerId.options[
            elementInformation.mainFormFields.drpCustomerId.selectedIndex].text}</div>
            <div style="margin-top: 3px">Customer BIN: ${genUtil.getDataAttributeValue.dropdownSelected(
                elementInformation.mainFormFields.drpCustomerId,
                dataAttributes.customer.bin)}</div>
            <div style="margin-top: 3px">Customer Address: ${genUtil.getDataAttributeValue.dropdownSelected(
                    elementInformation.mainFormFields.drpCustomerId,
                    dataAttributes.customer.address)}</div>
            <div style="margin-top: 3px">Shipping Address: ${elementInformation.mainFormFields.txtShippingAddress.value
        }</div>
            <div>Driver Name : ${elementInformation.mainFormFields.txtDriverName.value}</div>
            <div>Driver Mobile No. : ${elementInformation.mainFormFields.txtDriverMobile.value}</div>
            <div>Vehicle Type : ${elementInformation.mainFormFields.drpVehicleTypeId.options[elementInformation
            .mainFormFields.drpVehicleTypeId.selectedIndex].text}</div>
            <div>Vehicle Registration No. : ${elementInformation.mainFormFields.txtVehicleRegistrationNo.value}</div>
          </div>

          <div class="col-4 mt-3">

            <div style="margin-top: 3px">Invoice No.: </div>
            <div style="margin-top: 3px">Invoice Date:</div>
            <div>Sales Date : </div>
            <div>Challan No. : </div>
            <div style="margin-top: 3px">Issue Date: </div>
            <div style="margin-top: 3px">Issue Time: </div>
          </div>
          <div class="col-12 mt-3 draft-watermark">
            <table class="table table-bordered">
              <tbody>
                <tr class="text-center">
                  <td>SL</td>

                  <td>
                    Description of product or service (with brand name if
                    applicable)
                  </td>
                  <td>Unit of Supply</td>
                  <td>Quantity</td>
                  <td>Unit Price<sup>1</sup> (Taka)</td>
                  <td>Total Price<sup>1</sup> (Taka)</td>
                  <td>SD Rate</td>
                  <td>SD Amount (Taka)</td>
                  <td>Value Added Tax Rate / Specific Tax</td>
                  <td>Amount of Value Added Tax/Specified Tax</td>
                  <td>Price Including All Kinds of Duties and Taxes</td>
                </tr>
                <tr class="text-center">
                  <td>(1)</td>
                  <td>(2)</td>
                  <td>(3)</td>
                  <td>(4)</td>
                  <td>(5)</td>
                  <td>(6)</td>
                  <td>(7)</td>
                  <td>(8)</td>
                  <td>(9)</td>
                  <td>(10)</td>
                  <td>(11)</td>
                </tr>
                ${salesDetailsText}
                <tr>
                  <td colspan="5" class="total-text-cell text-right">Total</td>
                  <td class="amount-quantity-cell">${commonUtilitySale.getTotalPrice()}</td>
                  <td class="amount-quantity-cell"></td>
                  <td class="amount-quantity-cell">${commonUtilitySale.getTotalSd()}</td>
                  <td class="amount-quantity-cell"></td>
                  <td class="amount-quantity-cell">${commonUtilitySale.getTotalVat()}</td>
                  <td class="amount-quantity-cell">${commonUtilitySale.getTotalPriceWithVatAfterDiscount()}</td>
                </tr>
                
              </tbody>
            </table>
          </div>

          <div class="col-12" style="margin-top: 100px; padding-left: 20px">
            

            <div>
              * In case of deductible supply at source, the form will be
              considered as integrated tax invoice and tax deduction certificate
              at source and it will be applicable for deductible supply at
              source.
            </div>
          </div>

          <div class="col-12 mt-2">
            <div class="row">
              <div class="col-3">
                <hr class="black-hr" />
                <sup>1</sup>Prices excluding all types of taxes
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
`;
    return draftText;
}