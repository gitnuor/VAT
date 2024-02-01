/// <reference path="../site.js" />

const genUtil = window.generalUtility;

//All List of Array and Props Name
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        changeQuantity: 'changeQuantity',
        changeUnitPrice: 'changeUnitPrice',
        changeAmount: 'changeAmount',
        changeVatPercent: 'changeVatPercent',
        changeVat: 'changeVat',
        returnSdPercent: 'returnSdPercent',
        returnSd: 'returnSd',
    },
}

//Data Attribute Id Assign
const dataAttributes = {
    product: {
        measurementUnitId: 'data-Measurement-Unit-Id',
        measurementUnitName: 'data-Measurement-Unit-Name',
        sD: 'data-Sd',
        vAt: 'data-Vat',
        quantity: 'data-Quantity',
        unitPrice: 'data-UnitPrice',
        salesDetailId: 'data-SalesDetailId',
    },
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
    },
    mainFormFields: {
        hdnSalesId: document.getElementById('SalesId'),
        txtInvoiceNo: document.getElementById('InvoiceNo'),
        dateInvoiceDate: document.getElementById('InvoiceDate'),
        dateChangeDate: document.getElementById('ChangeDate'),
        txtClientNoteNo: document.getElementById('ClientNoteNo'),
        dateClientNoteTime: document.getElementById('ClientNoteTime'),
        drpVehicleTypeId: document.getElementById('VehicleTypeId'),
        txtVehicleRegNo: document.getElementById('VehicleRegNo'),
        txtVehicleDriverName: document.getElementById('VehicleDriverName'),
        txtVehicleDriverContactNo: document.getElementById('VehicleDriverContactNo')
    },
    productField: {
        drpSalesDetailId: document.getElementById('SalesDetailId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        quantity: document.getElementById('Quantity'),
        unitPrice: document.getElementById('UnitPrice'),
        amount: document.getElementById('Amount'),
        vatPercent: document.getElementById('VatPercent'),
        vat: document.getElementById('Vat'),
        sdPercent: document.getElementById('SdPercent'),
        sd: document.getElementById('Sd'),
        changeQuantity: document.getElementById('QuantityToChange'),
        changeUnitPrice: document.getElementById('ChangeAmountPerItem'),
        changeAmount: document.getElementById('ChangeAmount'),
        changeVatPercent: document.getElementById('ChangeVatPercent'),
        changeVat: document.getElementById('ChangeVat'),
        returnSdPercent: document.getElementById('ChangeSdPercent'),
        returnSd: document.getElementById('ChangeSd'),
        reasonOfReturn: document.getElementById('ReasonOfChangeInDetail'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        hdnSalesDetailId: document.getElementById('SalesDetailId'),
    },
    listTables: {
        product: document.getElementById('productTable'),
        productBody: document.getElementById('productTableBody'),
    },
    btn: {
        btnAddProduct: document.getElementById('btnAddProduct'),
        btnResetProduct: document.getElementById('btnResetProduct'),
        btnSave: document.getElementById('btnSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
    totalCalculatedTableCell: {
        changeQuantity: document.getElementById('changeQuantity'),
        changeUnitPrice: document.getElementById('changeUnitPrice'),
        changeAmount: document.getElementById('changeAmount'),
        changeVatPercent: document.getElementById('changeVatPercent'),
        changeVat: document.getElementById('changeVat'),
        returnSdPercent: document.getElementById('ChangeSdPercent'),
        returnSd: document.getElementById('ChangeSd')
    }
}
const priceCalculation = {
    calculatePrice: (unitPrice, qty) => {
        return generalUtility.roundNumberOption.byFour(unitPrice * qty);
    },
    calculateSd: (price, sdPercent) => {
        return generalUtility.roundNumberOption.byFour(price * sdPercent / 100);
    },
    calculateVat: (price, vatPercent) => {
        return generalUtility.roundNumberOption.byFour(price * vatPercent / 100);
    }
}
const commonUtilityCreditNote = {

    removeProduct: (infoId) => {
        listInfo.products = generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
        commonUtilityCreditNote.showTotalAmount();
    },
    getTotalReturnQuantity: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeQuantity);
    },

    getTotalReturnUnitPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeUnitPrice);
    },

    getTotalReturnAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeAmount);
    },
    getTotalReturnVatPercent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeVatPercent);
    },

    getTotalReturnVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeVat);
    },

    getTotalReturnSdPercent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnSdPercent);
    },
    getTotalReturnSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnSd);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.changeQuantity.innerText = commonUtilityCreditNote.getTotalReturnQuantity();
        elementInformation.totalCalculatedTableCell.changeUnitPrice.innerText = commonUtilityCreditNote.getTotalReturnUnitPrice();
        elementInformation.totalCalculatedTableCell.changeAmount.innerText = commonUtilityCreditNote.getTotalReturnAmount();
        elementInformation.totalCalculatedTableCell.changeVatPercent.innerText = commonUtilityCreditNote.getTotalReturnVatPercent();
        elementInformation.totalCalculatedTableCell.changeVat.innerText = commonUtilityCreditNote.getTotalReturnVat();
        elementInformation.totalCalculatedTableCell.returnSdPercent.innerText = commonUtilityCreditNote.getTotalReturnSdPercent();
        elementInformation.totalCalculatedTableCell.returnSd.innerText = commonUtilityCreditNote.getTotalReturnSd();
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let productIndex = 0;
        listInfo.products.forEach(d => {
            formData.append(`DetailList[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`DetailList[${productIndex}].Quantity`, d.quantity);
            formData.append(`DetailList[${productIndex}].UnitPrice`, d.unitPrice);
            formData.append(`DetailList[${productIndex}].Amount`, d.amount);
            formData.append(`DetailList[${productIndex}].VatPercent`, d.vatPercent);
            formData.append(`DetailList[${productIndex}].Vat`, d.vat);
            formData.append(`DetailList[${productIndex}].SdPercent`, d.sdPercent);
            formData.append(`DetailList[${productIndex}].Sd`, d.sd);
            formData.append(`DetailList[${productIndex}].QuantityToChange`, d.changeQuantity);
            formData.append(`DetailList[${productIndex}].ChangeAmountPerItem`, d.changeUnitPrice);
            formData.append(`DetailList[${productIndex}].ChangeAmount`, d.changeAmount);
            formData.append(`DetailList[${productIndex}].ChangeVatPercent`, d.changeVatPercent);
            formData.append(`DetailList[${productIndex}].ChangeVat`, d.changeVat);
            formData.append(`DetailList[${productIndex}].ChangeSdPercent`, d.returnSdPercent);
            formData.append(`DetailList[${productIndex}].ChangeSd`, d.returnSd);
            formData.append(`DetailList[${productIndex}].ReasonOfChangeInDetail`, d.reasonOfReturn);
            formData.append(`DetailList[${productIndex}].SalesDetailId`, d.salesDetailId);

            productIndex++;
        });
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: '/Sales/PriceChangeCreditNoteSave',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                //console.log(d);
                window.location.href = `/Sales/Index`;
            },
            error: function (d) {
                //console.log(d);
            }
        });
    },
     addRelatedProducts: () => {
        const productInfo = {
            productInfoId: window.generalUtility.getRandomString(6),
            salesDetailId: +elementInformation.productField.drpSalesDetailId.value,
            productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField.drpSalesDetailId),
            measurementUnitId: elementInformation.productField.hdnMeasurementUnitId.value,
            measurementUnitName: elementInformation.productField.txtMeasurementUnitName.value,
            quantity: elementInformation.productField.quantity.value,
            unitPrice: elementInformation.productField.unitPrice.value,
            amount: elementInformation.productField.amount.value,
            vatPercent: elementInformation.productField.vatPercent.value,
            vat: elementInformation.productField.vat.value,
            sdPercent: elementInformation.productField.sdPercent.value,
            sd: elementInformation.productField.sd.value,
            changeQuantity: elementInformation.productField.changeQuantity.value,
            changeUnitPrice: elementInformation.productField.changeUnitPrice.value,
            changeAmount: elementInformation.productField.changeAmount.value,
            changeVatPercent: elementInformation.productField.changeVatPercent.value,
            changeVat: elementInformation.productField.changeVat.value,
            returnSdPercent: elementInformation.productField.returnSdPercent.value,
            returnSd: elementInformation.productField.returnSd.value,
            reasonOfReturn: elementInformation.productField.reasonOfReturn.value,
        }

        listInfo.products.push(productInfo);

        const dataRow = `
            <tr id="tr_${productInfo.productInfoId}">
                <td>
                    ${productInfo.productName}
                </td>
                 <td> ${productInfo.measurementUnitName}</td>
                 <td> ${productInfo.quantity}</td>
                 <td> ${productInfo.unitPrice}</td>
                 <td> ${productInfo.amount}</td>
                 <td> ${productInfo.vatPercent}</td>
                 <td> ${productInfo.vat}</td>
                 <td> ${productInfo.sdPercent}</td>
                 <td> ${productInfo.sd}</td>
                 <td> ${productInfo.changeQuantity}</td>
                 <td> ${productInfo.changeUnitPrice}</td>
                 <td> ${productInfo.changeAmount}</td>
                 <td> ${productInfo.changeVatPercent}</td>
                 <td> ${productInfo.changeVat}</td>
                 <td> ${productInfo.returnSdPercent}</td>
                 <td> ${productInfo.returnSd}</td>
                 <td> ${productInfo.reasonOfReturn}</td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityCreditNote.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
        elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
         commonChangeEventsForCalculation.resetForm.product();
         commonUtilityCreditNote.showTotalAmount();
    }
}

const commonChangeEventsForCalculation = {
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpSalesDetailId);
        },
    },

    QtyChangeEvent: (unitPrice, qty, vatPercent, sdPercent ) => {
        let price = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.changeAmount.value = price;
        elementInformation.productField.changeVat.value = priceCalculation.calculateVat(price, vatPercent);
        elementInformation.productField.returnSd.value = priceCalculation.calculateSd(price, sdPercent);
    }
}


const setProductInfoByProductId = (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    elementInformation.productField.hdnMeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);
    elementInformation.productField.txtMeasurementUnitName.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);
    const vatPercent = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vAt);
    elementInformation.productField.vatPercent.value = vatPercent;
    const sdPercent = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.sD);
    const quantity = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.quantity);
    const unitPrice = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.unitPrice);
    elementInformation.productField.sdPercent.value = sdPercent;
    elementInformation.productField.quantity.value = quantity;
    elementInformation.productField.unitPrice.value = unitPrice;
    const price = priceCalculation.calculatePrice(unitPrice, quantity);
    elementInformation.productField.amount.value = price;
    elementInformation.productField.vat.value = priceCalculation.calculateVat(price, vatPercent);
    elementInformation.productField.sd.value = priceCalculation.calculateSd(price, sdPercent);
    elementInformation.productField.returnSdPercent.value = sdPercent;
    elementInformation.productField.changeUnitPrice.value = unitPrice;
    elementInformation.productField.changeVatPercent.value = vatPercent;
    elementInformation.productField.hdnSalesDetailId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.salesDetailId);

}

//Check is Duplicate Product
const isProductExit = (salesDetailId) => {
    console.log(listInfo);
    if (listInfo.products.find(f => f.salesDetailId == salesDetailId)) {
        return true;
    }
    return false;
}
//Call Add Product Method Call
elementInformation.btn.btnAddProduct.addEventListener('click', () => {
    if (!isProductExit(elementInformation.productField.drpSalesDetailId.value)) {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmProductInformation))
            commonUtilityCreditNote.addRelatedProducts();
    } else {
        toastr.error('Product Already Exist');
    }
});


//Call Set Product Related Field Method
elementInformation.productField.drpSalesDetailId.addEventListener('change', (event) => {
    setProductInfoByProductId(event);
});

//Save Information
elementInformation.btn.btnSave.addEventListener('click', () => {
    if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
        if (listInfo.products.length <= 0) {
            window.generalUtility.message.showErrorMsg("Can not save without product!!!");
            return;
        } else {
            commonUtilityCreditNote.postFormData(commonUtilityCreditNote.getFormData());
        }

    }
});

//Save as a draft information
elementInformation.btn.btnSaveDraft.addEventListener('click', () => {
    if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
        if (listInfo.products.length <= 0) {
            window.generalUtility.message.showErrorMsg("Can not save without product!!!");
            return;
        } else {
            commonUtilityCreditNote.postFormData(commonUtilityCreditNote.getFormData());
        }
    }
});

//Reset Whole form
elementInformation.btn.btnResetForm.addEventListener('click', () => {
    location.reload();
});


elementInformation.btn.btnResetProduct.addEventListener('click', () => {
    commonChangeEventsForCalculation.resetForm.product();
});



elementInformation.productField.changeQuantity.addEventListener('input', (event) => {

    const unitPrice = elementInformation.productField.changeUnitPrice.value;
    const qty = elementInformation.productField.changeQuantity.value;
    const vatPercent = elementInformation.productField.changeVatPercent.value;
    const sdPercent = elementInformation.productField.returnSdPercent.value;
    commonChangeEventsForCalculation.QtyChangeEvent(unitPrice, qty, vatPercent, sdPercent);
});



//check change quantity cannot be greater than original quantity
elementInformation.productField.changeQuantity.addEventListener('change', (event) => {
    const qtyOriginal = +elementInformation.productField.quantity.value;
    const qtyChange = +elementInformation.productField.changeQuantity.value;
    if (qtyChange > qtyOriginal) {
        genUtil.message.showErrorMsg("Change quantity should be less than or equal to original quantity!");
    }
    
});


//check change unit price cannot be greater than original price
elementInformation.productField.changeUnitPrice.addEventListener('change', (event) => {
    const priceOriginal = +elementInformation.productField.unitPrice.value;
    const priceChange = +elementInformation.productField.changeUnitPrice.value;
    if (priceChange > priceOriginal) {
        genUtil.message.showErrorMsg("Change price should be less than or equal to original price!");
    }

});

