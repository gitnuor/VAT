//All List of Array and Props Name
/// <reference path="site.js" />

const genUtil = window.generalUtility;

const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        returnQuantity: 'returnQuantity',
        returnUnitPrice: 'returnUnitPrice',
        returnAmount: 'returnAmount',
        returnVatParcent: 'returnVatParcent',
        returnVat: 'returnVat',
        returnSdParcent: 'returnSdParcent',
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
    productMeasurement: {
        productMeasurementConvertionRatio: 'data-product-convertion-ratio'
    },
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
    },
    mainFormFields: {
        drpVehicleTypeId: document.getElementById('VehicleTypeId'),
        txtVoucherNo: document.getElementById('VoucherNo'),
        txtVehicleRegNo: document.getElementById('VehicleRegNo'),
        dateReturnDate: document.getElementById('ReturnDate'),
        hdnSalesId: document.getElementById('SalesId'),
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        quantity: document.getElementById('Quantity'),
        unitPrice: document.getElementById('UnitPrice'),
        amount: document.getElementById('Amount'),
        vatParcent: document.getElementById('VatParcent'),
        vat: document.getElementById('Vat'),
        sdParcent: document.getElementById('SdParcent'),
        sd: document.getElementById('Sd'),
        returnQuantity: document.getElementById('ReturnQuantity'),
        returnUnitPrice: document.getElementById('ReturnUnitPrice'),
        returnAmount: document.getElementById('ReturnAmount'),
        returnVatParcent: document.getElementById('ReturnVatParcent'),
        returnVat: document.getElementById('ReturnVat'),
        returnSdParcent: document.getElementById('ReturnSdParcent'),
        returnSd: document.getElementById('ReturnSd'),
        reasonOfReturn: document.getElementById('ReasonOfReturn'),
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
        returnQuantity: document.getElementById('returnQuantity'),
        returnUnitPrice: document.getElementById('returnUnitPrice'),
        returnAmount: document.getElementById('returnAmount'),
        returnVatParcent: document.getElementById('returnVatParcent'),
        returnVat: document.getElementById('returnVat'),
        returnSdParcent: document.getElementById('returnSdParcent'),
        returnSd: document.getElementById('returnSd')
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
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnQuantity);
    },
    getTotalReturnUnitPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnUnitPrice);
    },
    getTotalReturnAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnAmount);
    },
    getTotalReturnVatParcent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnVatParcent);
    },
    getTotalReturnVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnVat);
    },
    getTotalReturnSdParcent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnSdParcent);
    },
    getTotalReturnSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.returnSd);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.returnQuantity.innerText = commonUtilityCreditNote.getTotalReturnQuantity();
        elementInformation.totalCalculatedTableCell.returnUnitPrice.innerText = commonUtilityCreditNote.getTotalReturnUnitPrice();
        elementInformation.totalCalculatedTableCell.returnAmount.innerText = commonUtilityCreditNote.getTotalReturnAmount();
        elementInformation.totalCalculatedTableCell.returnVatParcent.innerText = commonUtilityCreditNote.getTotalReturnVatParcent();
        elementInformation.totalCalculatedTableCell.returnVat.innerText = commonUtilityCreditNote.getTotalReturnVat();
        elementInformation.totalCalculatedTableCell.returnSdParcent.innerText = commonUtilityCreditNote.getTotalReturnSdParcent();
        elementInformation.totalCalculatedTableCell.returnSd.innerText = commonUtilityCreditNote.getTotalReturnSd();
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let productIndex = 0;
        listInfo.products.forEach(d => {
            console.log(productIndex);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ProductId`, d.productId);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].Quantity`, d.quantity);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].UnitPrice`, d.unitPrice);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].Amount`, d.amount);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].VatParcent`, d.vatParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].Vat`, d.vat);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].SdParcent`, d.sdParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].Sd`, d.sd);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnQuantity`, d.returnQuantity);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnUnitPrice`, d.returnUnitPrice);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnAmount`, d.returnAmount);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnVatParcent`, d.returnVatParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnVat`, d.returnVat);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnSdParcent`, d.returnSdParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnSd`, d.returnSd);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReasonOfReturnInDetail`, d.reasonOfReturn);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].SalesDetailId`, d.salesDetailId);

            productIndex++;
        });
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: '/Sales/CreditNoteSave',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                console.log(d);
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
            productId: +elementInformation.productField.drpProductId.value,
            productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField.drpProductId),
            measurementUnitId: elementInformation.productField.hdnMeasurementUnitId.value,
            measurementUnitName: elementInformation.productField.txtMeasurementUnitName.value,
            quantity: elementInformation.productField.quantity.value,
            unitPrice: elementInformation.productField.unitPrice.value,
            amount: elementInformation.productField.amount.value,
            vatParcent: elementInformation.productField.vatParcent.value,
            vat: elementInformation.productField.vat.value,
            sdParcent: elementInformation.productField.sdParcent.value,
            sd: elementInformation.productField.sd.value,
            returnQuantity: elementInformation.productField.returnQuantity.value,
            returnUnitPrice: elementInformation.productField.returnUnitPrice.value,
            returnAmount: elementInformation.productField.returnAmount.value,
            returnVatParcent: elementInformation.productField.returnVatParcent.value,
            returnVat: elementInformation.productField.returnVat.value,
            returnSdParcent: elementInformation.productField.returnSdParcent.value,
            returnSd: elementInformation.productField.returnSd.value,
            reasonOfReturn: elementInformation.productField.reasonOfReturn.value,
            salesDetailId: elementInformation.productField.hdnSalesDetailId.value,
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
                 <td> ${productInfo.vatParcent}</td>
                 <td> ${productInfo.vat}</td>
                 <td> ${productInfo.sdParcent}</td>
                 <td> ${productInfo.sd}</td>
                 <td> ${productInfo.returnQuantity}</td>
                 <td> ${productInfo.returnUnitPrice}</td>
                 <td> ${productInfo.returnAmount}</td>
                 <td> ${productInfo.returnVatParcent}</td>
                 <td> ${productInfo.returnVat}</td>
                 <td> ${productInfo.returnSdParcent}</td>
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
    },
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

const commonChangeEventsForCalculation = {
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
            let unitDrpElements = `<option value>Select Unit</option>`;
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
        },
    },

    QtyChangeEvent: (unitPrice, qty, vatParcent, sdParcent ) => {
        let price = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.returnAmount.value = price;
        elementInformation.productField.returnVat.value = priceCalculation.calculateVat(price, vatParcent);
        elementInformation.productField.returnSd.value = priceCalculation.calculateSd(price, sdParcent);
    }
}


const setProductInfoByProductId = (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    /*elementInformation.productField.hdnMeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);*/
    elementInformation.productField.txtMeasurementUnitName.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);
    const vatParcent = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vAt);
    elementInformation.productField.vatParcent.value = vatParcent;
    const sdParcent = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.sD);
    const quantity = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.quantity);
    const unitPrice = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.unitPrice);
    elementInformation.productField.sdParcent.value = sdParcent;
    elementInformation.productField.quantity.value = quantity;
    elementInformation.productField.unitPrice.value = unitPrice;
    const price = priceCalculation.calculatePrice(unitPrice, quantity);
    elementInformation.productField.amount.value = price;
    elementInformation.productField.vat.value = priceCalculation.calculateVat(price, vatParcent);
    elementInformation.productField.sd.value = priceCalculation.calculateSd(price, sdParcent);
    elementInformation.productField.returnSdParcent.value = sdParcent;
    elementInformation.productField.returnUnitPrice.value = unitPrice;
    elementInformation.productField.returnVatParcent.value = vatParcent;
    elementInformation.productField.hdnSalesDetailId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.salesDetailId);

}

const setMeasurementUnitByProduct = (event) => {
    let unitDrpElements = `<option value>Select Unit</option>`;
    const productId = event.target.value;
    if (productId) {
        commonUtilityCreditNote.getMeasurementUnitForProduct(productId).then(products => {
            console.log(products);
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

//Check is Duplicate Product
const isProductExit = (productId) => {
    if (listInfo.products.find(f => f.productId == productId)) {
        return true;
    }
    return false;
}
//Call Add Product Method Call
elementInformation.btn.btnAddProduct.addEventListener('click', () => {
    if (!isProductExit(elementInformation.productField.drpProductId.value)) {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmProductInformation))
            commonUtilityCreditNote.addRelatedProducts();
    } else {
        toastr.error('Product Already Exist');
    }
});


//Call Set Product Related Field Method
elementInformation.productField.drpProductId.addEventListener('change', (event) => {
    setProductInfoByProductId(event);
    setMeasurementUnitByProduct(event);
});

elementInformation.productField.hdnMeasurementUnitId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        const measurementDataAttr = dataAttributes.productMeasurement;
        //commonChangeEventsForCalculation.clearQuantityEvent();
        const prodFld = elementInformation.productField;
        // change current stock according to measurement unit
       
        prodFld.quantity.value = (
            (+genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, prodDataAttr.quantity))
            *
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        );
        // change current stock according to measurement unit
        prodFld.unitPrice.value = genUtil.roundNumberOption.byNumber((
            (+genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, prodDataAttr.unitPrice))
            /
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        ), 8);
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



elementInformation.productField.returnQuantity.addEventListener('input', (event) => {
    const unitPrice = elementInformation.productField.returnUnitPrice.value;
    const qty = elementInformation.productField.returnQuantity.value;
    const vatParcent = elementInformation.productField.returnVatParcent.value;
    const sdParcent = elementInformation.productField.returnSdParcent.value;
    commonChangeEventsForCalculation.QtyChangeEvent(unitPrice, qty, vatParcent, sdParcent);
});