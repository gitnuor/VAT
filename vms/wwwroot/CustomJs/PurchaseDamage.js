//All List of Array and Props Name
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
    },
}

//Data Attribute Id Assign
const dataAttributes = {
    product: {
        purchaseDetailId: 'data-Purchase-Detail-Id',
        measurementUnitId: 'data-Measurement-Unit-Id',
    },
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
    },
    mainFormFields: {
        txtVoucherNo: document.getElementById('VoucherNo'),
        txtNbrverifierName: document.getElementById('NbrverifierName'),
        txtNbrverifierDesignation: document.getElementById('NbrverifierDesignation'),
        txtDescription: document.getElementById('Description'),
        hdnPurchaseId: document.getElementById('PurchaseId')
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtDamageQty: document.getElementById('DamageQty'),
        txtUsableQty: document.getElementById('UsableQty'),
        txtSuggestedNewUnitPrice: document.getElementById('SuggestedNewUnitPrice'),
        txtDamageDescription: document.getElementById('DamageDescription'),
        hdnPurchaseDetailId: document.getElementById('PurchaseDetailId'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
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
}

const commonUtilityDamage = {

    removeProduct: (infoId) => {
        listInfo.products = generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },


    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let productIndex = 0;
        listInfo.products.forEach(d => {
            console.log(productIndex);
            formData.append(`PurchaseId`, elementInformation.mainFormFields.hdnPurchaseId.value);
            formData.append(`DamageDetailList[${productIndex}].ProductId`, d.productId);
            formData.append(`DamageDetailList[${productIndex}].DamageQty`, d.damageQty);
            formData.append(`DamageDetailList[${productIndex}].UsableQty`, d.usableQty);
            formData.append(`DamageDetailList[${productIndex}].SuggestedNewUnitPrice`, d.suggestedNewUnitPrice);
            formData.append(`DamageDetailList[${productIndex}].DamageDescription`, d.damageDescription);
            formData.append(`DamageDetailList[${productIndex}].PurchaseDetailId`, d.purchaseDetailId);
            formData.append(`DamageDetailList[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            productIndex++;
        });
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'Damage',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                console.log(d);
                window.location.href = `/Purchase/Index`;
            },
            error: function (d) {
                console.log(d);
            }
        });
    },
     addRelatedProducts: () => {
        const productInfo = {
            productInfoId: window.generalUtility.getRandomString(6),
            productId: +elementInformation.productField.drpProductId.value,
            productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField.drpProductId),
            damageQty: elementInformation.productField.txtDamageQty.value,
            usableQty: elementInformation.productField.txtUsableQty.value,
            suggestedNewUnitPrice: elementInformation.productField.txtSuggestedNewUnitPrice.value,
            damageDescription: elementInformation.productField.txtDamageDescription.value,
            purchaseDetailId: elementInformation.productField.hdnPurchaseDetailId.value,
            measurementUnitId: elementInformation.productField.hdnMeasurementUnitId.value,
        }

        listInfo.products.push(productInfo);

        const dataRow = `
            <tr id="tr_${productInfo.productInfoId}">
                <td>
                    ${productInfo.productName}
                </td>
                 <td> ${productInfo.damageQty}</td>
                 <td> ${productInfo.usableQty}</td>
                 <td> ${productInfo.suggestedNewUnitPrice}</td>
                 <td> ${productInfo.damageDescription}</td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityDamage.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
        elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
        commonChangeEventsForCalculation.resetForm.product();
    }
}

const commonChangeEventsForCalculation = {
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
        },
    },
}


const setProductInfoByProductId = (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    elementInformation.productField.hdnPurchaseDetailId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.purchaseDetailId);
    elementInformation.productField.hdnMeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);
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
            commonUtilityDamage.addRelatedProducts();
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
            commonUtilityDamage.postFormData(commonUtilityDamage.getFormData());
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
            commonUtilityDamage.postFormData(commonUtilityDamage.getFormData());
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
