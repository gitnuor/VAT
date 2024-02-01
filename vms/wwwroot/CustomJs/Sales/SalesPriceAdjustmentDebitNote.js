//All List of Array and Props Name
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        changeQuantity: 'changeQuantity',
        changeUnitPrice: 'changeUnitPrice',
        changeAmount: 'changeAmount',
        changeVatParcent: 'changeVatParcent',
        changeVat: 'changeVat',
        changeSdParcent: 'changeSdParcent',
        changeSd: 'changeSd',
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
        changeQuantity: document.getElementById('ReturnQuantity'),
        changeUnitPrice: document.getElementById('ReturnUnitPrice'),
        changeAmount: document.getElementById('ReturnAmount'),
        changeVatParcent: document.getElementById('ReturnVatParcent'),
        changeVat: document.getElementById('ReturnVat'),
        changeSdParcent: document.getElementById('ReturnSdParcent'),
        changeSd: document.getElementById('ReturnSd'),
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
        changeQuantity: document.getElementById('changeQuantity'),
        changeUnitPrice: document.getElementById('changeUnitPrice'),
        changeAmount: document.getElementById('changeAmount'),
        changeVatParcent: document.getElementById('changeVatParcent'),
        changeVat: document.getElementById('changeVat'),
        changeSdParcent: document.getElementById('changeSdParcent'),
        changeSd: document.getElementById('changeSd')
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
    getTotalReturnVatParcent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeVatParcent);
    },

    getTotalReturnVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeVat);
    },

    getTotalReturnSdParcent: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeSdParcent);
    },
    getTotalReturnSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.changeSd);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.changeQuantity.innerText = commonUtilityCreditNote.getTotalReturnQuantity();
        elementInformation.totalCalculatedTableCell.changeUnitPrice.innerText = commonUtilityCreditNote.getTotalReturnUnitPrice();
        elementInformation.totalCalculatedTableCell.changeAmount.innerText = commonUtilityCreditNote.getTotalReturnAmount();
        elementInformation.totalCalculatedTableCell.changeVatParcent.innerText = commonUtilityCreditNote.getTotalReturnVatParcent();
        elementInformation.totalCalculatedTableCell.changeVat.innerText = commonUtilityCreditNote.getTotalReturnVat();
        elementInformation.totalCalculatedTableCell.changeSdParcent.innerText = commonUtilityCreditNote.getTotalReturnSdParcent();
        elementInformation.totalCalculatedTableCell.changeSd.innerText = commonUtilityCreditNote.getTotalReturnSd();
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
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnQuantity`, d.changeQuantity);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnUnitPrice`, d.changeUnitPrice);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnAmount`, d.changeAmount);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnVatParcent`, d.changeVatParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnVat`, d.changeVat);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnSdParcent`, d.changeSdParcent);
            formData.append(`vmSalesCreditNoteDetials[${productIndex}].ReturnSd`, d.changeSd);
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
            changeQuantity: elementInformation.productField.changeQuantity.value,
            changeUnitPrice: elementInformation.productField.changeUnitPrice.value,
            changeAmount: elementInformation.productField.changeAmount.value,
            changeVatParcent: elementInformation.productField.changeVatParcent.value,
            changeVat: elementInformation.productField.changeVat.value,
            changeSdParcent: elementInformation.productField.changeSdParcent.value,
            changeSd: elementInformation.productField.changeSd.value,
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
                 <td> ${productInfo.changeQuantity}</td>
                 <td> ${productInfo.changeUnitPrice}</td>
                 <td> ${productInfo.changeAmount}</td>
                 <td> ${productInfo.changeVatParcent}</td>
                 <td> ${productInfo.changeVat}</td>
                 <td> ${productInfo.changeSdParcent}</td>
                 <td> ${productInfo.changeSd}</td>
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
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
        },
    },

    QtyChangeEvent: (unitPrice, qty, vatParcent, sdParcent ) => {
        let price = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.changeAmount.value = price;
        elementInformation.productField.changeVat.value = priceCalculation.calculateVat(price, vatParcent);
        elementInformation.productField.changeSd.value = priceCalculation.calculateSd(price, sdParcent);
    }
}


const setProductInfoByProductId = (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    elementInformation.productField.hdnMeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);
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
    elementInformation.productField.changeSdParcent.value = sdParcent;
    elementInformation.productField.changeUnitPrice.value = unitPrice;
    elementInformation.productField.changeVatParcent.value = vatParcent;
    elementInformation.productField.hdnSalesDetailId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.salesDetailId);

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
    const vatParcent = elementInformation.productField.changeVatParcent.value;
    const sdParcent = elementInformation.productField.changeSdParcent.value;
    commonChangeEventsForCalculation.QtyChangeEvent(unitPrice, qty, vatParcent, sdParcent);
});