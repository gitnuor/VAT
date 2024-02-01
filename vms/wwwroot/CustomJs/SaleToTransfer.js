const formInfo= {
    frmMainInformation: document.getElementById('frmMainInformation'),
    frmProductInformation: document.getElementById('frmProductInformation'),
    frmDocument: document.getElementById('frmDocument')
}

const classesToShowHideInEvents = {
    vehicleRegistrationNo:'vehicle-registration-no',
}


classesToShowHideInEvents.vehicleRelatedList =
    [classesToShowHideInEvents.vehicleRegistrationNo];


const commonChangeEventsForDisplay = {
    hideVehicleSpecialOptions: () => {
        classesToShowHideInEvents.vehicleRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    displayVehicleRegistrationNo: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.vehicleRegistrationNo);
    },
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
    IsStockAvailable: (curStock,qty) => {
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
        if (commonValidation.IsQtyNegetiveOrZero(qty)) {
            toastr.error("Value Zero or Negetive not Allow");
        }
        if (commonValidation.IsStockAvailable(curStock, qty)) {
            toastr.error("Cur. Stock not Available for this Qty");
        }
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
            formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(productInformationField.drpProductId);
        },
        doc: () => {
            formInfo.frmDocument.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(fileAddControl.drpFileType);
        }
    },
}

const dataAttributes = {
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
        vechileIsRequireRegistration:'data-Is-Require-Registration'
    },
    branch: {
        address:'data-address'
    }
}
const mainFormFields = {
    drpOrgBranchId: document.getElementById('OrgBranchId'),
    drpToOrgBranchId: document.getElementById('ToOrgBranchId'),
    drpSalesDeliveryTypeId: document.getElementById('SalesDeliveryTypeId'),
    dateDeliveryDate: document.getElementById('DeliveryDate'),
    txtShippingAddress: document.getElementById('ShippingAddress'),
    drpVehicleTypeId: document.getElementById('VehicleTypeId'),
    txtDriverName: document.getElementById('DriverName'),
    txtDriverMobile: document.getElementById('DriverMobile'),
    txtVehicleRegistrationNo: document.getElementById('VehicleRegistrationNo'),
    txtSalesRemarks: document.getElementById('SalesRemarks'),
}

const totalCalculatedTableCell= {
    productTotalPrice: document.getElementById('productTotalPrice'),
}

const commonUtilitySale = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(productInformationField.ProductInformationList, productInformationField.prop.totalPrice);
    },
    showTotalAmount: () => {
        totalCalculatedTableCell.productTotalPrice.innerText = commonUtilitySale.getTotalPrice();
    }
}

const productInformationField = {
    drpProductId: document.getElementById('ProductId'),
    txtCurrentStock: document.getElementById('CurrentStock'),
    txtQuantity: document.getElementById('Quantity'),
    txtTotalPrice: document.getElementById('TotalPrice'),
    txtUnitPrice: document.getElementById('UnitPrice'),
    drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
    txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
    ProductInformationList: [],
    tbdProduct: document.getElementById('tbdProduct'),
    prop: { totalPrice:'TotalPrice'}
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

const DrowDownListMaker = (toId, name, Routid, url) => {
    $(toId).val = '';
    var s = '<option value="0">' + name + '</option>';
    $(toId).html(s);
    $.ajax({
        type: "GET",
        url: location.origin + url + Routid,
        data: "{}",
        cache: false,
        success: function (data) {
            console.log(name);
            console.log(data);
            var s = '<option value="0">' + name + '</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].orgBranchId +
                    '" data-address="' + data[i].address + '">'
                    + data[i].name + '</option>';
            }
            console.log(s);
            console.log(toId);
            $(toId).html(s);
        }
    });
}
const getBranchWithOutSelfId = (selfBranchId) => {
    DrowDownListMaker(mainFormFields.drpToOrgBranchId, 'Select Branch', selfBranchId, "/Sales/GetOrgBranchWithOutSelf/");
}

mainFormFields.drpOrgBranchId.addEventListener('change', (event) => {
    const selfBranchId = mainFormFields.drpOrgBranchId.value;
    getBranchWithOutSelfId(selfBranchId);
});


mainFormFields.drpToOrgBranchId.addEventListener('change', (event) => {
    alert('hello');
    const address = window.generalUtility.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.branch.address);
    alert(addess)
})


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



const setProductInfoByProductId = () => {
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
    setProductInfoByProductId();
});


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
    commonChangeEventsForValidation.QtyChangeEvent(curStock,qty);
});

//Total Price
productInformationField.txtTotalPrice.addEventListener('input', (event) => {
    commonChangeEventsForCalculation.totalPriceChangeEvent(+event.target.value);
});

const addRelatedProducts = () => {
    const productName = productInformationField.drpProductId.options[productInformationField.drpProductId.selectedIndex].text;

    const newProductRndId = Math.random().toString(36).substring(2);
    const newProductInfo = {
        RndId: newProductRndId,
        ProductId: productInformationField.drpProductId.value,
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
            <td>${productInformationField.txtMeasurementUnitName.value}</td>
            <td>${productInformationField.txtCurrentStock.value}</td>
            <td>${productInformationField.txtUnitPrice.value}</td>
            <td>${productInformationField.txtQuantity.value}</td>
            <td>${productInformationField.txtTotalPrice.value}</td>
  <td><a onclick="removeRelatedProduct('${newProductRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    productInformationField.tbdProduct.insertAdjacentHTML('beforeend', newProductDataRow);
    commonUtilitySale.showTotalAmount();
    commonChangeEventsForCalculation.resetForm.product();
}

const removeRelatedProduct = (productRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${productRndId}`);
    fileDisplayRowId.remove();
    productInformationField.ProductInformationList = productInformationField.ProductInformationList.filter(f => f.RndId !== productRndId);
    commonUtilitySale.showTotalAmount();
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
    console.log('ttt');
    console.log(data);
    //disContentById('loaderAnimation');
    $.ajax({
        url: 'SaleToTransfer',
        data: data,
        cache: false,
        method: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (d) {
            console.log(d);
            window.location.href = `Details/${d.id}`;
        },
        error: function () {
            console.log(d);
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
    formData.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
    formData.append("OrgBranchId", mainFormFields.drpOrgBranchId.value);
    formData.append("ToOrgBranchId", mainFormFields.drpToOrgBranchId.value);
    
    formData.append("SalesDeliveryTypeId", mainFormFields.drpSalesDeliveryTypeId.value);
    formData.append("DeliveryDate", mainFormFields.dateDeliveryDate.value);
    formData.append("ShippingAddress", mainFormFields.txtShippingAddress.value);
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
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].CurrentStock`, relatedPro.CurrentStock);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].Quantity`, relatedPro.Quantity);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].TotalPrice`, relatedPro.TotalPrice);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].UnitPrice`, relatedPro.UnitPrice);
        formData.append(`VmSaleExportDetails[${relatedProductIndex}].MeasurementUnitId`, relatedPro.MeasurementUnitId);
        relatedProductIndex++;
    }
    saveData(formData);
}

//Start Make Form Data to Object

btn.btnResetProduct.addEventListener('click', () => {
    commonChangeEventsForCalculation.resetForm.product();
});