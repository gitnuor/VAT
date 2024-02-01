const genUtil = window.generalUtility;
const taxTypeCombined = 8;
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        totalPrice: 'totalPrice',
    },
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
    },
    mainFormFields: {
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        drpPurchaseReasonId: document.getElementById('PurchaseReasonId'),
        drpVendorId: document.getElementById('VendorId'),
        txtBranch: document.getElementById('Branch'),
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtProductDescription: document.getElementById('ProductDescription'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtQuantity: document.getElementById('Quantity'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtUnitPrice: document.getElementById('UnitPrice'),
    },  
    totalCalculatedTableCell: {
        productTotalPrice: document.getElementById('productTotalPrice'),
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

const priceCalculation = {
    calculatePrice: (unitPrice, qty) => {
        return window.generalUtility.roundNumberOption.byFour(unitPrice * qty);
    },
    calculateUnitPrice: (price, qty) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(price / qty);
    },
}

const changeEffect = {
    priceChange: (totalPrice) => {
        const cd = priceCalculation.calculateCd(totalPrice,
            +(elementInformation.productField.txtCustomDutyPercent.value));
        elementInformation.productField.txtCustomDuty.value = cd;

        const rd = priceCalculation.calculateRd(totalPrice,
            +(elementInformation.productField.txtRegulatoryDutyPercent.value));
        elementInformation.productField.txtRegulatoryDuty.value = rd;

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const ait = priceCalculation.calculateAit(totalPrice,
            +(elementInformation.productField.txtAdvanceIncomeTaxPercent.value));
        elementInformation.productField.txtAdvanceIncomeTax.value = ait;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, cd, sd, rd);
        elementInformation.productField.txtVatAblePrice.value = vatablePrice;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const priceWithVat = priceCalculation.calculatePriceIncludingAllTax(vatablePrice, vat, ait);
        elementInformation.productField.txtTotalPriceIncludingAllTax.value = priceWithVat;
    },
    taxChange: () => {
        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        const ait = priceCalculation.calculateAit(totalPrice,
            +(elementInformation.productField.txtAdvanceIncomeTaxPercent.value));
        elementInformation.productField.txtAdvanceIncomeTax.value = ait;

        const cd = priceCalculation.calculateCd(totalPrice,
            +(elementInformation.productField.txtCustomDutyPercent.value));
        elementInformation.productField.txtCustomDuty.value = cd;

        const id = priceCalculation.calculateId(totalPrice,
            +(elementInformation.productField.txtImportDutyPercent.value));
        elementInformation.productField.txtImportDuty.value = id;

        const rd = priceCalculation.calculateRd(totalPrice,
            +(elementInformation.productField.txtRegulatoryDutyPercent.value));
        elementInformation.productField.txtRegulatoryDuty.value = rd;

        const sdImposableValue = priceCalculation.calculateSdImposablePrice(totalPrice, id, cd, rd);

        const sd = priceCalculation.calculateSd(sdImposableValue,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const vatImposablePrice = priceCalculation.calculateVatablePrice(totalPrice, id, cd, sd, rd);
        elementInformation.productField.txtVatAblePrice.value = vatImposablePrice;

        const vat = priceCalculation.calculateVat(vatImposablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const atImposablePrice = priceCalculation.calculateAdvanceTaxImposableValue(totalPrice, id, cd, sd, rd);
        elementInformation.productField.txtAdvanceTax.value = atImposablePrice;

        const at = priceCalculation.calculateAdvanceTax(atImposablePrice,
            +(elementInformation.productField.txtAdvanceTaxPercent.value));
        elementInformation.productField.txtProductVat.value = at;

        const priceWithVat = priceCalculation.calculatePriceIncludingAllTax(vatImposablePrice, vat, ait);
        elementInformation.productField.txtTotalPriceIncludingAllTax.value = priceWithVat;
    },
    productValueOrTaxChange: () => {
        const totalPrice = +elementInformation.productField.txtTotalPrice.value;
    },
    notifyForNotifiablePriceChange: (productId, productName, unitPrice) => {
        console.log(window.mainUrls.product);
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfFinishedProductsWithNotifiableChange/?productId=${productId
                }&unitPrice=${unitPrice}`,
            cache: false,
            method: 'GET',
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend',
                        `<div class="alert alert-warning price-alert" role="alert">Price of ${productName
                        } has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declare again.</div>`);
                }
            }
        });
    }
}

const commonChangeEventsForCalculation = {
    removePaymentEvent: () => {
        listInfo.payments = [];
        elementInformation.listTables.paymentBody.innerHTML = '';
        commonUtilityPurchase.showTotalPaidAmount();
    },
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.txtTotalPrice.value = totalPrice;

        changeEffect.productValueOrTaxChange();
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +elementInformation.productField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        elementInformation.productField.txtUnitPrice.value = unitPrice;

        changeEffect.productValueOrTaxChange();
    },
   
    clearPriceEvent: () => {
        elementInformation.productField.txtUnitPrice.value = 0;
        elementInformation.productField.txtQuantity.value = 0;
        elementInformation.productField.txtTotalPrice.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
            let unitDrpElements = `<option value>Select Unit</option>`;
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
        },
    }
}

const specialItem = {
 
    elementToValidate: {
        productChange: [
            elementInformation.productField.drpProductId, elementInformation.productField.drpProductVatTypeId
        ]
    }
}



const commonUtilityPurchase = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice);
    },
    removeProduct: (infoId) => {
        listInfo.products =
            generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        commonUtilityPurchase.showTotalAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.productTotalPrice.innerText = commonUtilityPurchase.getTotalPrice();
        
    },
    showTotalPaidAmount: () => {
        /*Change here*/
        elementInformation.totalPaidCalculatedTableCell.paymentTotalPaid.innerText =
            new Intl.NumberFormat('en-IN').format(commonUtilityPurchase.getTotalPaidAmount());
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let docIndex = 0;
        let productIndex = 0;
        let paymentIndex = 0;
        let taxPaymentIndex = 0;
        listInfo.products.forEach(d => {
            formData.append(`Products[${productIndex}].ProductId`, d.productId);
            formData.append(`Products[${productIndex}].ProductDescription`, d.description);
           
            formData.append(`Products[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`Products[${productIndex}].MeasurementUnitName`, d.measurementUnitName);
            formData.append(`Products[${productIndex}].Quantity`, d.quantity);
            formData.append(`Products[${productIndex}].UnitPrice`, d.unitPrice);
            
            formData.append(`Products[${productIndex}].ProductVatTypeId`, 1);
           
            productIndex++;
        });
        
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'PurchaseImportSubscription',
            data: data,
            cache: false,
            method: 'POST',
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                window.location.href = `Details/${d.id}`;
            },
            error: function (d) {
                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnSave, 'disabled');
            }
        });
    },
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

const dataAttributes = {
    product: {
        measurementUnitId: 'data-mu-id',
        measurementUnitName: 'data-mu-name',
       
    },
}

//Product change event
elementInformation.productField.drpProductId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        elementInformation.productField.txtMeasurementUnitName.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);

        commonChangeEventsForCalculation.clearPriceEvent();
       /* window.generalUtility.validateElement.elementByElementList(specialItem.elementToValidate.productChange);*/
        setMeasurementUnitByProduct(event);
    });


const setMeasurementUnitByProduct = (event) => {
    
    let unitDrpElements = `<option value>Select Unit</option>`;
    const productId = event.target.value;
    if (productId) {
        commonUtilityPurchase.getMeasurementUnitForProduct(productId).then(products => {
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
        commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
    });

//Total Price
elementInformation.productField.txtTotalPrice.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.totalPriceChangeEvent(+event.target.value);
    });


//Add Product
elementInformation.btn.btnAddProduct.addEventListener('click',
    () => {
        if (!isProductExit(elementInformation.productField.drpProductId.value)) {
            if (window.$('#frmProductInformation').valid())
                addRelatedProducts();
        } else {
            toastr.error('Product Already Exist');
        }
    });


//Save Information

elementInformation.btn.btnSave.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                window.generalUtility.message.showErrorMsg('Can not save without product!!!');
                return;
            }

            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSave, 'disabled', 'disabled');
            //todo
            commonUtilityPurchase.postFormData(commonUtilityPurchase.getFormData());
        }
    });

//Save as a draft information
elementInformation.btn.btnSaveDraft.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                window.generalUtility.message.showErrorMsg('Can not save as draft without product!!!');
                return;
            }
        }
    });

//Reset Whole form
elementInformation.btn.btnResetForm.addEventListener('click',
    () => {
        location.reload();
    });


//Add Product List


const addRelatedProducts = () => {
    const productInfo = {
        productInfoId: window.generalUtility.getRandomString(6),
        productId: elementInformation.productField.drpProductId.value,
        productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField
            .drpProductId),
        description: elementInformation.productField.txtProductDescription.value,
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
        
    }

    listInfo.products.push(productInfo);

    const dataRow = `
        <tr id="tr_${productInfo.productInfoId}">
            <td>${productInfo.productName}</td>
            <td style="text-align:center">${productInfo.measurementUnitName}</td>
            <td class="text-end">${productInfo.unitPriceToDisplay}</td>
            <td class="text-end">${productInfo.quantityToDisplay}</td>
            <td class="text-end">${productInfo.totalPriceToDisplay}</td>
            <td>
                <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
            </td>
        </tr>
    `;
    elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
    commonUtilityPurchase.showTotalAmount();
    commonChangeEventsForCalculation.resetForm.product();
    changeEffect.notifyForNotifiablePriceChange(productInfo.productId, productInfo.productName, productInfo.unitPrice);
}

const removeRelatedProduct = (productInfoId) => {
    const fileDisplayRowId = document.getElementById(`row_${productInfoId}`);
    fileDisplayRowId.remove();
    listInfo.products = listInfo.products.filter(f => f.RndId !== productInfoId);
}

const isProductExit = (productId) => {
    if (listInfo.products.find(f => f.ProductId === productId)) {
        return true;
    }
    return false;
}
//End  Product List


//saveData Request Start
const saveData = (data) => {
    $.ajax({
        url: 'PurchaseImportSubscription',
        data: data,
        cache: false,
        method: 'POST',
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (d) {
            window.location.href = `Details/${d.id}`;
        },
        error: function () {
        }
    });
}
//saveData Request End


elementInformation.btn.btnResetProduct.addEventListener('click',
    () => {
        commonChangeEventsForCalculation.resetForm.product();
    });