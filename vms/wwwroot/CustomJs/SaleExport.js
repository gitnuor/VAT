const genUtil = window.generalUtility;

const formInfo = {
    frmMainInformation: document.getElementById('frmMainInformation'),
    frmProductInformation: document.getElementById('frmProductInformation'),
    frmPayment: document.getElementById('frmPayment'),
    frmDocument: document.getElementById('frmDocument')
}

const classesToShowHideInEvents = {
    vdsRelated: 'vds-related',
    bankPayment: 'payment-bank',
    mobilePayment: 'payment-mobile',
    vehicleRegistrationNo: 'vehicle-registration-no'
}

const otherBlock = {
    notificationArea: document.getElementById('notificationArea')
}

classesToShowHideInEvents.paymentRelatedList =
    [classesToShowHideInEvents.bankPayment, classesToShowHideInEvents.mobilePayment];

classesToShowHideInEvents.vehicleRelatedList =
    [classesToShowHideInEvents.vehicleRegistrationNo];

const classesOfDueAmount = [window.generallyUsedCssClass.successText, window.generallyUsedCssClass.warningText, window.generallyUsedCssClass.dangerText];
const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    hideVehicleSpecialOptions: () => {
        classesToShowHideInEvents.vehicleRelatedList.forEach(element => {
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
    displayVehicleRegistrationNo: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.vehicleRegistrationNo);
    },
    notifyForNotifiablePriceChange: (productId, productName) => {
        //console.log(window.mainUrls.product);
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfRawMaterialWithNotifiableChange/?productId=${productId}`,
            cache: false,
            method: "GET",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend', `<div class="alert alert-warning" role="alert">Price of element of ${productName} has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declear again.</div>`);
                }
            }
        });
    }
}

const priceCalculation = {
    calculatePrice: (unitPrice, qty) => {
        return generalUtility.roundNumberOption.byFour(unitPrice * qty);
    },
    calculateUnitPrice: (price, qty) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return generalUtility.roundNumberOption.byFour(price / qty);
    },

}

const commonValidation = {
    IsStockAvailable: (curStock, qty) => {
        if (curStock >= qty) {
            return false;
        }
        return true;
    },
    IsQtyNegetiveOrZero: (qty) => {
        if (qty <= 0) {
            return true;
        }
        return false;
    }
}

const commonChangeEventsForValidation = {
    QtyChangeEvent: (curStock, qty) => {
        //if (commonValidation.IsQtyNegetiveOrZero(qty)) {
        //    toastr.error("Value Zero or Negetive not Allow");
        //}
        //if (commonValidation.IsStockAvailable(curStock, qty)) {
        //    toastr.error("Cur. Stock not Available for this Qty");
        //}
    }
}

const commonChangeEventsForCalculation = {
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        productInformationField.txtTotalPrice.value = totalPrice;
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +productInformationField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        productInformationField.txtUnitPrice.value = unitPrice;
    },
    clearPriceEvent: () => {
        productField.txtUnitPrice.value = 0;
        productField.txtQuantity.value = 0;
        productField.txtTotalPrice.value = 0;
        productField.txtDiscountPerItem.value = 0;
        productField.txtTotalProductDiscount.value = 0;
        productField.txtProductPriceWithDiscount.value = 0;
        productField.txtProductSupplementaryDuty.value = 0;
        productField.txtVatAblePrice.value = 0;
        productField.txtProductVat.value = 0;
        productField.txtProductPriceWithVat.value = 0;
        productField.txtProductPriceWithVatAfterDiscount.value = 0;
    },
    resetForm: {
        product: () => {
            formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(productInformationField.drpProductId);
            let unitDrpElements = `<option value>Select Unit</option>`;
            genUtil.setDropdownValue.selectPickerUpdateItems(productInformationField.drpMeasurementUnitId,
                unitDrpElements);
        },
        payment: () => {
            formInfo.frmPayment.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(paymentMethodAddControl.drpPaymentMethodId);
            window.generalUtility.setDropdownValue.selectPickerReset(paymentMethodAddControl.drpBankId);
        },
        doc: () => {
            formInfo.frmDocument.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(fileAddControl.drpFileType);
        }
    },
    notifyForNotifiablePriceChange: (productId, productName) => {
        //console.log(window.mainUrls.product);
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfRawMaterialWithNotifiableChange/?productId=${productId}`,
            cache: false,
            method: "GET",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend', `<div class="alert alert-warning price-alert" role="alert">Price of element of ${productName} has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declare again.</div>`);
                }
            }
        });
    }
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
        productProductVatTypeId: 'data-product-vat-type',
        productDefaultVatPercent: 'data-product-default-vat-percent',
        productSupplementaryDutyPercent: 'data-product-sd-percent',
        productMaxSaleQty: 'data-product-max-sale-quantity',
        productMeasurementUnitId: 'data-product-measurement-unit-id',
        productMeasurementUnitName: 'data-product-measurement-unit-name'
    },
    vechile: {
        vechileIsRequireRegistration: 'data-Is-Require-Registration'
    },
    customer: {
        recName: 'data-rec-name',
        cusMobile: 'data-cus-mobile',
        address: 'data-address',
        recMobile: 'data-rec-mobile',
        delCountry: 'data-del-country'
    },
    productMeasurement: {
        productMeasurementConvertionRatio: 'data-product-convertion-ratio'
    },
}
const mainFormFields = {
    drpOrgBranchId: document.getElementById('OrgBranchId'),
    drpCustomerId: document.getElementById('CustomerId'),
    txtCustomerPhoneNumber: document.getElementById('CustomerPhoneNumber'),
    txtWorkOrderNo: document.getElementById('WorkOrderNo'),
    drpSalesDeliveryTypeId: document.getElementById('SalesDeliveryTypeId'),
    dateDeliveryDate: document.getElementById('DeliveryDate'),
    txtReceiverName: document.getElementById('ReceiverName'),
    txtReceiverContactNo: document.getElementById('ReceiverContactNo'),
    txtShippingAddress: document.getElementById('ShippingAddress'),
    drpShippingCountryId: document.getElementById('ShippingCountryId'),
    txtLcNo: document.getElementById('LcNo'),
    dateLcDate: document.getElementById('LcDate'),
    txtBillOfEntry: document.getElementById('BillOfEntry'),
    dateBillOfEntryDate: document.getElementById('BillOfEntryDate'),
    dateDueDate: document.getElementById('DueDate'),
    txtTermsOfLc: document.getElementById('TermsOfLc'),
    txtPONo: document.getElementById('PONo'),
    drpExportTypeId: document.getElementById('ExportTypeId'),
    drpVehicleTypeId: document.getElementById('VehicleTypeId'),
    txtDriverName: document.getElementById('DriverName'),
    txtDriverMobile: document.getElementById('DriverMobile'),
    txtVehicleRegistrationNo: document.getElementById('VehicleRegistrationNo'),
    txtSalesRemarks: document.getElementById('SalesRemarks'),
}

const totalCalculatedTableCell = {
    productTotalPrice: document.getElementById('productTotalPrice'),
}
const totalPaidCalculatedTableCell = {
    paymentTotalPaid: document.getElementById('paymentTotalPaid')
}

const commonUtilitySale = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(productInformationField.ProductInformationList, productInformationField.prop.totalPrice);
    },
    getTotalPaidAmount: () => {
        return generalUtility.getSumFromObjectArray(paymentMethodAddControl.RelatedPaymentList, paymentMethodAddControl.prop.paidAmount);
    },
    showTotalAmount: () => {
        totalCalculatedTableCell.productTotalPrice.innerText = commonUtilitySale.getTotalPrice();
    },
    showTotalPaidAmount: () => {
        /*Changed here*/
        totalPaidCalculatedTableCell.paymentTotalPaid.innerText = new Intl.NumberFormat('en-IN').format(commonUtilitySale.getTotalPaidAmount());
    },
    showDueAmount: () => {
        const dueBlock = paymentMethodAddControl.dueAmount;

        window.generalUtility.displayOption.removeMultipleCssClass(dueBlock, classesOfDueAmount);
        const payableAmount = commonUtilitySale.getTotalPrice();
        const dueAmount = payableAmount - commonUtilitySale.getTotalPaidAmount();
        if (dueAmount < 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.dangerText);
        } else if (dueAmount > 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.warningText);
        } else {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.successText);
        }

        paymentMethodAddControl.dueAmount.innerText = `Payable Amount: ${payableAmount}; Due amount: ${dueAmount}`;
        paymentMethodAddControl.paidAmount.value = dueAmount;
    },
    getProductsForSale: async (branchId) => {
        const url = `/api/salesproducts/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

const productInformationField = {
    drpProductId: document.getElementById('ProductId'),
    txtSKUId: document.getElementById('SKUId'),
    txtSKUNo: document.getElementById('SKUNo'),
    txtCurrentStock: document.getElementById('CurrentStock'),
    txtQuantity: document.getElementById('Quantity'),
    txtTotalPrice: document.getElementById('TotalPrice'),
    txtUnitPrice: document.getElementById('UnitPrice'),
    drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
    txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
    ProductInformationList: [],
    tbdProduct: document.getElementById('tbdProduct'),
    prop: { totalPrice: 'TotalPrice' }
}


const paymentMethodAddControl = {
    drpPaymentMethodId: document.getElementById('PaymentMethodId'),
    drpBankId: document.getElementById('bankId'),
    paidAmount: document.getElementById('paidAmount'),
    paymentDate: document.getElementById('paymentDate'),
    tbdPayment: document.getElementById('tbdPayment'),
    txtMobilePaymentWalletNo: document.getElementById('mobilePaymentWalletNo'),
    txtPaymentDocumentOrTransDate: document.getElementById('paymentDocumentOrTransDate'),
    txtPaymentRemarks: document.getElementById('paymentRemarks'),
    txtDocumentNoOrTransactionId: document.getElementById('documentNoOrTransactionId'),
    RelatedPaymentList: [],
    dueAmount: document.getElementById('dueAmount'),
    prop: { paidAmount: 'PaidAmount' }
}

const fileAddControl = {
    fileDocument: document.getElementById('fileDocument'),
    drpFileType: document.getElementById('drpFileType'),
    txtDocumentRemarks: document.getElementById('DocumentRemarks'),
    tbdAttachedFiles: document.getElementById('tbdAttachedFiles'),
    RelatedDocumentList: []
}

const fileUploadValidationMessage = {
    spnFileType: document.getElementById('msgFileType'),
    spnFileDocument: document.getElementById('msgFileDocument')
}

const btn = {
    btnAddRelatedFile: document.getElementById('btnAddRelatedFile'),
    btnAddProduct: document.getElementById('btnAddProduct'),
    btnAddRelatedPayment: document.getElementById('btnAddRelatedPayment'),
    btnSave: document.getElementById('save'),
    btnResetProduct: document.getElementById('btnResetProduct')
}

btn.btnAddRelatedFile.addEventListener('click', () => {
    if (window.generalUtility.validateElement.formByForm(formInfo.frmDocument))
        addRelatedFiles();
});

btn.btnSave.addEventListener('click', () => {
    if (window.generalUtility.validateElement.formByForm(formInfo.frmMainInformation)) {
        generalUtility.alterAttr.setAttr(btn.btnSave, 'disabled', 'disabled');
        sendToSaleLocalController();
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
        commonChangeEventsForCalculation.resetForm.doc();
    }
}

const removeRelatedFile = (fileRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${fileRndId}`);
    fileDisplayRowId.remove();
    fileAddControl.RelatedDocumentList = fileAddControl.RelatedDocumentList.filter(f => f.RndId !== fileRndId);
}



const setProductInfoByProductId = (event) => {
    const maxSaleQty = window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.product.productMaxSaleQty);
    productInformationField.txtCurrentStock.value = maxSaleQty;
    const MeasurementUnitId = window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.product.productMeasurementUnitId);
    productInformationField.drpMeasurementUnitId.value = MeasurementUnitId;
    const MeasurementUnitName = window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.product.productMeasurementUnitName);
    productInformationField.txtMeasurementUnitName.value = MeasurementUnitName;
    const UnitPrice = window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.product.productSalesUnitPrice);
    productInformationField.txtUnitPrice.value = UnitPrice;
}

productInformationField.drpProductId.addEventListener('change', (event) => {
    setProductInfoByProductId(event);
    setMeasurementUnitByProduct(event);
});

mainFormFields.drpOrgBranchId.addEventListener('change',
    (event) => {
        let prodDrpElements = `<option value>Select Product</option>`;
        ////let custDrpElements = `<option value>Select Customer</option>`;
        const branchId = event.target.value;
        if (branchId)
        {
            
            commonUtilitySale.getProductsForSale(branchId).then(products => {
                //console.log(products);
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
                genUtil.setDropdownValue.selectPickerUpdateItems(productInformationField.drpProductId,
                    prodDrpElements);

            });

           

        }
        //else
        //{
        //    genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpCustomerId,
        //        custDrpElements);
        //}
        //commonChangeEventsForCalculation.removeProductForBranchChange();
        commonChangeEventsForCalculation.resetForm.product();
    });


    // set measurement unit by product change

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
            genUtil.setDropdownValue.selectPickerUpdateItems(productInformationField.drpMeasurementUnitId,
                unitDrpElements);
        }).then(() => {
            genUtil.setDropdownValue.selectPickerByControl(productInformationField.drpMeasurementUnitId,
                genUtil.getDataAttributeValue.dropdownSelected(productInformationField.drpProductId, dataAttributes.product.productMeasurementUnitId));
        });

    } else {
        genUtil.setDropdownValue.selectPickerUpdateItems(productInformationField.drpMeasurementUnitId,
            unitDrpElements);
    }

}


//Unit Price
productInformationField.txtUnitPrice.addEventListener('input', (event) => {
    const unitPrice = +event.target.value;
    const qty = +(productInformationField.txtQuantity.value);
    commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
});

//Quantity
productInformationField.txtQuantity.addEventListener('input', (event) => {
    const unitPrice = +(productInformationField.txtUnitPrice.value);
    const curStock = +(productInformationField.txtCurrentStock.value);
    const qty = +event.target.value;
    commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);

    commonChangeEventsForValidation.QtyChangeEvent(curStock, qty);
});

//Total Price
productInformationField.txtTotalPrice.addEventListener('input', (event) => {
    commonChangeEventsForCalculation.totalPriceChangeEvent(+event.target.value);
});


//  measurement unit change event
productInformationField.drpMeasurementUnitId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        const measurementDataAttr = dataAttributes.productMeasurement;
        //commonChangeEventsForCalculation.clearQuantityEvent();
        const prodFld = productInformationField;
        // change current stock according to measurement unit
        prodFld.txtCurrentStock.value = (
            (+genUtil.getDataAttributeValue.dropdownSelected(productInformationField.drpProductId, prodDataAttr.productMaxSaleQty))
            *
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        );
        // change current stock according to measurement unit
        prodFld.txtUnitPrice.value = genUtil.roundNumberOption.byNumber((
            (+genUtil.getDataAttributeValue.dropdownSelected(productInformationField.drpProductId, prodDataAttr.productSalesUnitPrice))
            /
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        ), 8);
    });

const addRelatedProducts = () => {
    const productName = productInformationField.drpProductId.options[productInformationField.drpProductId.selectedIndex].text;

    const newProductRndId = Math.random().toString(36).substring(2);
    const newProductInfo = {
        RndId: newProductRndId,
        ProductId: productInformationField.drpProductId.value,
        sKUId: productInformationField.txtSKUId.value,
        sKUNo: productInformationField.txtSKUNo.value,
        CurrentStock: productInformationField.txtCurrentStock.value,
        Quantity: productInformationField.txtQuantity.value,
        TotalPrice: +(productInformationField.txtTotalPrice.value),
        UnitPrice: +(productInformationField.txtUnitPrice.value),
        MeasurementUnitId: productInformationField.drpMeasurementUnitId.value,
    }
    productInformationField.ProductInformationList.push(newProductInfo);
    const newProductDataRow = `
        <tr id="row_${newProductRndId}">
            <td>${productName}</td>
            <td>${productInformationField.txtSKUNo.value}</td>
            <td>${productInformationField.txtSKUId.value}</td>
            <td>${productInformationField.txtMeasurementUnitName.value}</td>
            <td class="text-right">${productInformationField.txtCurrentStock.value}</td>
            <td class="text-right">${productInformationField.txtUnitPrice.value}</td>
            <td class="text-right">${productInformationField.txtQuantity.value}</td>
            <td class="text-right">${productInformationField.txtTotalPrice.value}</td>
  <td><a onclick="removeRelatedProduct('${newProductRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    productInformationField.tbdProduct.insertAdjacentHTML('beforeend', newProductDataRow);
    commonUtilitySale.showTotalAmount();
    commonUtilitySale.showDueAmount();
    commonChangeEventsForCalculation.resetForm.product();
    commonChangeEventsForCalculation.notifyForNotifiablePriceChange(newProductInfo.productId, newProductInfo.productName);
}

const removeRelatedProduct = (productRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${productRndId}`);
    fileDisplayRowId.remove();
    productInformationField.ProductInformationList = productInformationField.ProductInformationList.filter(f => f.RndId !== productRndId);
    commonUtilitySale.showTotalAmount();
    commonUtilitySale.showDueAmount();
}
const isProductExit = (productId) => {
    if (productInformationField.ProductInformationList.find(f => f.ProductId == productId)) {
        return true;
    }
    return false;
}
btn.btnAddProduct.addEventListener('click', () => {
    if (!isProductExit(productInformationField.drpProductId.value)) {
        if (window.generalUtility.validateElement.formByForm(formInfo.frmProductInformation))
            addRelatedProducts();
    } else {
        toastr.error('Product Already Exist');
    }
});
btn.btnAddRelatedPayment.addEventListener('click', () => {
    if (window.generalUtility.validateElement.formByForm(formInfo.frmPayment))
        addRelatedPayments();
});
const addRelatedPayments = () => {
    const drpBankId = paymentMethodAddControl.drpBankId;
    const drpPaymentMethodId = paymentMethodAddControl.drpPaymentMethodId;
    let bankInfoId = null;
    let bankName = null;
    let paymentDocumentOrTransDate = null;
    let bankNameOrWalletDisplayValue = '';
    const PaymentMethodName = paymentMethodAddControl.drpPaymentMethodId.options[paymentMethodAddControl.drpPaymentMethodId.selectedIndex].text;
    const BankName = paymentMethodAddControl.drpBankId.options[paymentMethodAddControl.drpBankId.selectedIndex].text;

    const isBankingChannel =
        generalUtility.getInputValue.isValueTrue(window.generalUtility.getDataAttributeValue.dropdownSelected(drpPaymentMethodId, dataAttributes.payment.isBankingChannel));
    const isMobileTransaction =
        generalUtility.getInputValue.isValueTrue(window.generalUtility.getDataAttributeValue.dropdownSelected(drpPaymentMethodId, dataAttributes.payment.isMobileTransaction));
    if (isBankingChannel) {
        bankInfoId = drpBankId.value;
        bankName = window.generalUtility.getDataAttributeValue.dropdownSelectedText(paymentMethodAddControl
            .drpBankId);
        paymentDocumentOrTransDate = paymentMethodAddControl.txtPaymentDocumentOrTransDate.value;
        bankNameOrWalletDisplayValue = bankName;
    } else if (isMobileTransaction) {
        paymentDocumentOrTransDate = '';
        bankNameOrWalletDisplayValue = paymentMethodAddControl.txtMobilePaymentWalletNo.value;
    }
    const newPaymentMethodRndId = Math.random().toString(36).substring(2);
    const newPaymentMethodInfo = {
        RndId: newPaymentMethodRndId,
        BankId: paymentMethodAddControl.drpBankId.value,
        MobilePaymentWalletNo: paymentMethodAddControl.txtMobilePaymentWalletNo.value,
        DocumentNoOrTransactionId: paymentMethodAddControl.txtDocumentNoOrTransactionId.value,
        PaymentDocumentOrTransDate: paymentMethodAddControl.txtPaymentDocumentOrTransDate.value,
        PaymentMethodId: paymentMethodAddControl.drpPaymentMethodId.value,
        PaidAmount: +(paymentMethodAddControl.paidAmount.value),
        PaymentDate: paymentMethodAddControl.paymentDate.value,
        PaymentRemarks: paymentMethodAddControl.txtPaymentRemarks.value,
    }
    paymentMethodAddControl.RelatedPaymentList.push(newPaymentMethodInfo);
    const newPaymentMethodDataRow = `
        <tr id="row_${newPaymentMethodRndId}">
            <td>${PaymentMethodName}</td>
            <td>${bankNameOrWalletDisplayValue}</td>
            <td>${paymentMethodAddControl.txtDocumentNoOrTransactionId.value}</td>
            <td class="text-center">${paymentMethodAddControl.txtPaymentDocumentOrTransDate.value === null ? '' : paymentMethodAddControl.txtPaymentDocumentOrTransDate.value}</td>
            <td>${paymentMethodAddControl.paymentDate.value}</td>
            <td class="text-right">${paymentMethodAddControl.paidAmount.value}</td>
            <td>${paymentMethodAddControl.txtPaymentRemarks.value}</td>
            <td class="text-center">
 <a onclick="removeRelatedPaymentMethod('${newPaymentMethodRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    paymentMethodAddControl.tbdPayment.insertAdjacentHTML('beforeend', newPaymentMethodDataRow);
    commonUtilitySale.showTotalPaidAmount();
    commonUtilitySale.showDueAmount();
    commonChangeEventsForCalculation.resetForm.payment();
}

const removeRelatedPaymentMethod = (newPaymentMethodRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${newPaymentMethodRndId}`);
    fileDisplayRowId.remove();
    paymentMethodAddControl.RelatedPaymentList = paymentMethodAddControl.RelatedPaymentList.filter(f => f.RndId !== newPaymentMethodRndId);
    commonUtilitySale.showTotalPaidAmount();
    commonUtilitySale.showDueAmount();
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

mainFormFields.drpVehicleTypeId.addEventListener('change', (event) => {
    const isVehicleRegistrationNo =
        window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.vechile.vechileIsRequireRegistration);
    if (isVehicleRegistrationNo === 'True') {
        commonChangeEventsForDisplay.displayVehicleRegistrationNo();
    } else {
        mainFormFields.txtVehicleRegistrationNo.value = "";
        commonChangeEventsForDisplay.hideVehicleSpecialOptions();
    }
});

//saveData Request Start
const saveData = (data) => {
    //disContentById('loaderAnimation');
    $.ajax({
        url: 'SaleExport',
        data: data,
        cache: false,
        method: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (d) {
            //console.log(d);
            window.location.href = `Details/${d.id}`;
        },
        error: function () {
            //console.log(d);
            generalUtility.alterAttr.removeAttr(btn.btnSave, 'disabled');
        }
    });
}
//saveData Request End


//Start Make Form Data to Object
const sendToSaleLocalController = () => {
    let formData = new FormData();
    let relatedDocIndex = 0;
    let relatedProductIndex = 0;
    let relatedPaymentIndex = 0;
    formData.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
    formData.append("OrgBranchId", mainFormFields.drpOrgBranchId.value);
    formData.append("CustomerId", mainFormFields.drpCustomerId.value);
    formData.append("CustomerPhoneNumber", mainFormFields.txtCustomerPhoneNumber.value);
    formData.append("WorkOrderNo", mainFormFields.txtWorkOrderNo.value);
    formData.append("SalesDeliveryTypeId", mainFormFields.drpSalesDeliveryTypeId.value);
    formData.append("DeliveryDate", mainFormFields.dateDeliveryDate.value);
    formData.append("ReceiverName", mainFormFields.txtReceiverName.value);
    formData.append("ReceiverContactNo", mainFormFields.txtReceiverContactNo.value);
    formData.append("ShippingAddress", mainFormFields.txtShippingAddress.value);
    formData.append("drpShippingCountryId", mainFormFields.drpShippingCountryId.value);
    formData.append("LcNo", mainFormFields.txtLcNo.value);
    formData.append("LcDate", mainFormFields.dateLcDate.value);
    formData.append("BillOfEntry", mainFormFields.txtBillOfEntry.value);
    formData.append("BillOfEntryDate", mainFormFields.dateBillOfEntryDate.value);
    formData.append("TermsOfLc", mainFormFields.txtTermsOfLc.value);
    formData.append("DueDate", mainFormFields.dateDueDate.value);
    formData.append("TermsOfLc", mainFormFields.txtTermsOfLc.value);
    formData.append("PONo", mainFormFields.txtPONo.value);
    formData.append("ExportTypeId", mainFormFields.drpExportTypeId.value);
    formData.append("VehicleTypeId", mainFormFields.drpVehicleTypeId.value);
    formData.append("DriverName", mainFormFields.txtDriverName.value);
    formData.append("DriverMobile", mainFormFields.txtDriverMobile.value);
    formData.append("VehicleRegistrationNo", mainFormFields.txtVehicleRegistrationNo.value);
    formData.append("SalesRemarks", mainFormFields.txtSalesRemarks.value);

    for (const relatedDoc of fileAddControl.RelatedDocumentList) {
        formData.append(`VmSaleLocalDocuments[${relatedDocIndex}].DocumentType`, relatedDoc.DocumentTypeId);
        formData.append(`VmSaleLocalDocuments[${relatedDocIndex}].FileUpload`, relatedDoc.UploadedFile);
        formData.append(`VmSaleLocalDocuments[${relatedDocIndex}].DocumentRemarks`, relatedDoc.DocumentRemarks);
        relatedDocIndex++;
    }

    for (const relatedPro of productInformationField.ProductInformationList) {
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].ProductId`, relatedPro.ProductId);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].SKUId`, relatedPro.sKUId);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].SKUNo`, relatedPro.sKUNo);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].CurrentStock`, relatedPro.CurrentStock);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].Quantity`, relatedPro.Quantity);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].TotalPrice`, relatedPro.TotalPrice);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].UnitPrice`, relatedPro.UnitPrice);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].MeasurementUnitId`, relatedPro.MeasurementUnitId);
        relatedProductIndex++;
    }

    for (const relatedPay of paymentMethodAddControl.RelatedPaymentList) {
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].PaymentMethodId`, relatedPay.PaymentMethodId);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].BankId`, relatedPay.BankId);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].PaidAmount`, relatedPay.PaidAmount);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].PaymentDate`, relatedPay.PaymentDate);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].MobilePaymentWalletNo`, relatedPay.MobilePaymentWalletNo);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].PaymentDocumentDate`, relatedPay.PaymentDocumentDate);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].PaymentRemarks`, relatedPay.PaymentRemarks);
        formData.append(`VmSaleLocalPayments[${relatedPaymentIndex}].DocumentNoOrTransactionId`, relatedPay.DocumentNoOrTransactionId);
        relatedPaymentIndex++;
    }
    saveData(formData);
}

//Start Make Form Data to Object

btn.btnResetProduct.addEventListener('click', () => {
    commonChangeEventsForCalculation.resetForm.product();
});


mainFormFields.drpCustomerId.addEventListener('change', (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.customer;
    mainFormFields.txtReceiverContactNo.value = generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recMobile);

    mainFormFields.txtCustomerPhoneNumber.value = generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.cusMobile);

    mainFormFields.txtReceiverName.value = generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recName);

    mainFormFields.txtShippingAddress.value = generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.address);

    const delCountry = generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.delCountry);
    if (delCountry > 0) {
        generalUtility.setDropdownValue.selectPickerByControl(mainFormFields.drpShippingCountryId, delCountry);
    } else {
        generalUtility.setDropdownValue.selectPickerByControl(mainFormFields.drpShippingCountryId, "");
    }

});


const DrowDownListMaker = (toId, name, Routid, url) => {
    $(toId).val = '';
    var s = '<option value="">' + name + '</option>';
    $(toId).html(s);
    $.ajax({
        type: "GET",
        url: location.origin + url + Routid,
        data: "{}",
        cache: false,
        success: function (data) {
            var s = '<option value="">' + name + '</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].customerId +
                    '" data-address="' + data[i].deliveryAddress +
                    '" data-rec-mobile="' + data[i].deliveryContactPersonMobile +
                    '" data-rec-name="' + data[i].deliveryContactPerson +
                    '" data-cus-mobile="' + data[i].phoneNo +
                    '" data-del-country="' + data[i].deliveryCountryId +
                    '">'
                    + data[i].name + '</option>';
            }
            //$(toId).html(s);
            generalUtility.setDropdownValue.selectPickerUpdateItems(mainFormFields.drpCustomerId, s);
        }
    });

    //generalUtility.setDropdownValue.selectPickerReset(toId);
}
const GetCustomerByExportType = (exportTypeId) => {
    DrowDownListMaker(mainFormFields.drpCustomerId, 'Select Customer', exportTypeId, "/Sales/GetCustomerByExportType/");
}


mainFormFields.drpExportTypeId.addEventListener('change', (event) => {
    //const exportTypeId = event.target.value; // mainFormFields.drpExportTypeId.value;
    //GetCustomerByExportType(exportTypeId);
});