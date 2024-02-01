//All List of Array and Props Name
const genUtil = window.generalUtility;
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        totalPrice: 'totalPrice'
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
        frmDocument: document.getElementById('frmDocument')
    },
    mainFormFields: {
        drpSenderBranchId: document.getElementById('SenderBranchId'),
        drpReceiverBranchId: document.getElementById('ReceiverBranchId'),
        txtInvoiceNo: document.getElementById('InvoiceNo'),
        dateInvoiceDate: document.getElementById('InvoiceDate'),
        dateDeliveryDate: document.getElementById('DeliveryDate'),
        txtReceiverName: document.getElementById('ReceiverName'),
        txtReceiverContactNo: document.getElementById('ReceiverContactNo'),
        txtShippingAddress: document.getElementById('ShippingAddress'),
        drpVehicleTypeId: document.getElementById('VehicleTypeId'),
        txtDriverName: document.getElementById('DriverName'),
        txtDriverMobile: document.getElementById('DriverMobile'),
        txtVehicleRegistrationNo: document.getElementById('VehicleRegistrationNo'),
        txtBranchTransferSendRemarks: document.getElementById('BranchTransferSendRemarks'),
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
        txtUnitPrice: document.getElementById('UnitPrice'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtProductRemarks: document.getElementById('ProductRemarks'),
    },
    documentField: {
        drpDocumentTypeId: document.getElementById('DocumentType'),
        fileUploadedFile: document.getElementById('FileUpload'),
        txtDocumentRemarks: document.getElementById('DocumentRemarks')
    },
    totalCalculatedTableCell: {
        productTotalPrice: document.getElementById('productTotalPrice'),
    },
    listTables: {
        product: document.getElementById('productTable'),
        productBody: document.getElementById('productTableBody'),
        doc: document.getElementById('docTable'),
        docBody: document.getElementById('docTableBody')
    },
    btn: {
        btnAddProduct: document.getElementById('btnAddProduct'),
        btnResetProduct: document.getElementById('btnResetProduct'),
        btnAddDocument: document.getElementById('btnAddDocument'),
        btnSave: document.getElementById('btnSave'),
        btnFinalSave: document.getElementById('btnFinalSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
    otherBlock: {
        notificationArea: document.getElementById('notificationArea')
    },
    modal: {
        main: document.getElementById('BranchTransferSendSummeryModal'),
        body: document.getElementById('BranchTransferSendSummeryModalBody')
    }
}
//Special Item Id
const specialItem = {
    classesToShowHideInEvents: {
        vehicleRegistrationNo: 'vehicle-registration-no'
    },
    elementToValidate: {
        productChange: [
            elementInformation.productField.drpProductId
        ]
    }
}


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
    }
}

//Check Current Stock
const isCurrentStockAvailable = (qty, curStock) => {
    return qty > curStock;
}

//Call Current Stock method to  Check Available Stock
const isStock = (qty, curStock) => {
    const isInventoryStr = genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId,
        dataAttributes.product.isInventory);
    console.log(isInventoryStr);
    if (commonChangeEventsForCalculation.isCurStockAvailable(qty, curStock) && isInventoryStr === 'True') {


        showErrorMessage(validationMessage.msgQuantity, 'Qty not allow Greater then Current Stock');
        return false;
    }
    showErrorMessage(validationMessage.msgQuantity, '');
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
            commonUtilitySale.showTotalAmount();
            commonChangeEventsForCalculation.resetForm.product();
            genUtil.message.showErrorMsg(msg);
        }
    },
    removeProductForBranchChange: () => {
        commonChangeEventsForCalculation.removeProducts('All products has been removed due to remove change Branch.');
    },
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.txtTotalPrice.value = totalPrice;
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +elementInformation.productField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        elementInformation.productField.txtUnitPrice.value = unitPrice;
    },
    clearPriceEvent: () => {
        elementInformation.productField.txtUnitPrice.value = 0;
        elementInformation.productField.txtQuantity.value = 0;
        elementInformation.productField.txtTotalPrice.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
        },
        doc: () => {
            elementInformation.formInfo.frmDocument.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.documentField.drpDocumentTypeId);
        }
    },
    isCurStockAvailable: (qty, curStock) => {
        return isCurrentStockAvailable(qty, curStock);
    }
}

//Common Show or Hide for Change Event
const commonChangeEventsForDisplay = {
    hideVehicleSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.vehicleRelatedList.forEach(element => {
            genUtil.displayOption.hideItemByClassName(element);
        });
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
    removeProduct: (infoId) => {
        listInfo.products =
            generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        commonUtilitySale.showTotalAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removeDoc: (infoId) => {
        listInfo.docs = generalUtility.removeFromObjectArray(listInfo.docs, listInfo.docProp.documentInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.productTotalPrice.innerText = commonUtilitySale.getTotalPrice();
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let docIndex = 0;
        let productIndex = 0;
        console.log(listInfo.products);
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
            formData.append(`Products[${productIndex}].ProductRemarks`, d.remarks);
            productIndex++;
        });
        listInfo.docs.forEach(d => {
            formData.append(`Documents[${docIndex}].DocumentTypeId`, d.documentTypeId);
            formData.append(`Documents[${docIndex}].UploadedFile`, d.uploadedDocument);
            formData.append(`Documents[${docIndex}].DocumentRemarks`, d.documentRemarks);
            docIndex++;
        });
        formData.append(`BranchTransferSendRemarks`, elementInformation.mainFormFields.txtBranchTransferSendRemarks.value);
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'Create',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function(d) {
                console.log(d);
                window.location.href = `Details/${d.id}`;
            },
            error: function(d) {
                console.log(d);

                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnFinalSave, 'disabled');
            }
        });
    },
    postFormDataAsDraft: data => {
        window.$.ajax({
            url: 'CreateDraft',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function(d) {
                console.log(d);
                window.location.href = `Index`;
            },
            error: function(d) {
                console.log(d);

                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnFinalSave, 'disabled');
            }
        });
    },
    getProductsForSale: async (branchId) => {
        const url = `/api/salesproducts/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    }
}

//Data Attribute Id Assign
const dataAttributes = {
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
        isInventory: 'data-is-inventory'
    },
    vechile: {
        vechileIsRequireRegistration: 'data-Is-Require-Registration'
    },
    branch: {
        recName: 'data-rec-name',
        address: 'data-address',
        recMobile: 'data-rec-mobile'
    }
}


//Set Product Related Field Method
const setProductInfoByProductId = (event) => {

    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    const prodFld = elementInformation.productField;
    prodFld.txtCurrentStock.value = genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.productMaxSaleQty);

    prodFld.hdnMeasurementUnitId.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);

    prodFld.txtMeasurementUnitName.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);

    prodFld.txtUnitPrice.value =
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.productSalesUnitPrice);
}

//Call Set Product Related Field Method
elementInformation.productField.drpProductId.addEventListener('change',
    (event) => {
        setProductInfoByProductId(event);
    });


elementInformation.mainFormFields.drpSenderBranchId.addEventListener('change',
    (event) => {
        let prodDrpElements = `<option value>Select Product</option>`;
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
                            data-is-inventory='${element.isInventory}'>
                        ${element.productName}
                    </option>
                `;
                });
                genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.drpProductId, prodDrpElements);
            });
        } else {
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.drpProductId, prodDrpElements);
        }
        commonChangeEventsForCalculation.removeProductForBranchChange();
    });


elementInformation.mainFormFields.drpReceiverBranchId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.branch;
        elementInformation.mainFormFields.txtReceiverContactNo.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recMobile);

        elementInformation.mainFormFields.txtReceiverName.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.recName);

        elementInformation.mainFormFields.txtShippingAddress.value =
            genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.address);
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



//Add Product in Cart
const addRelatedProducts = () => {
    const productInfo = {
        productInfoId: genUtil.getRandomString(6),
        productId: +elementInformation.productField.drpProductId.value,
        currentStock: +elementInformation.productField.txtCurrentStock.value,
        currentStockToDisplay: +elementInformation.productField.txtCurrentStock.value,
        productName: genUtil.getDataAttributeValue.dropdownSelectedText(elementInformation.productField.drpProductId),
        description: elementInformation.productField.txtProductDescription.value,
        hsCode: elementInformation.productField.txtHscode.value,
        productCode: elementInformation.productField.txtProductCode.value,
        sKUId: elementInformation.productField.txtSKUId.value,
        sKUNo: elementInformation.productField.txtSKUNo.value,
        goodsId: elementInformation.productField.txtGoodsId.value,
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
        remarks: elementInformation.productField.txtProductRemarks.value,
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
                    ${productInfo.goodsId}
                </td>
                <td class="text-end">
                    ${productInfo.currentStockToDisplay}
                </td>
                <td>
                    ${productInfo.measurementUnitName}
                </td>
                <td class="text-end">
                    ${productInfo.quantityToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.unitPriceToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceToDisplay}
                </td>
                <td>
                    ${productInfo.remarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilitySale.removeProduct('${
        productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
    elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
    commonUtilitySale.showTotalAmount();
    commonChangeEventsForCalculation.resetForm.product();
}

//Check is Duplicate Product
const isProductExit = (productId) => {
    if (listInfo.products.find(f => f.productId === productId)) {
        return true;
    }
    return false;
}

//Listeners
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
        if (isVehicleRegistrationNo === 'True') {
            commonChangeEventsForDisplay.displayVehicleRegistrationNo();
        } else {
            elementInformation.mainFormFields.txtVehicleRegistrationNo.value = "";
            commonChangeEventsForDisplay.hideVehicleSpecialOptions();
        }
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
                const saleDraft = new window.bootstrap.Modal(elementInformation.modal.main,
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
                  <td class="product-remarks-cell">${item.remarks}</td>
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
            <div class="sub-header">Transfer Invoice of Centrally Registered Organization</div>
            <div class="sub-header">
              [Note clause (uma) of sub-rule (1) of rule 40]
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
            <div style="margin-top: 3px">
              Name and Address of Dispatching Branch/Store: ${elementInformation.mainFormFields.drpSenderBranchId.options[elementInformation
        .mainFormFields.drpSenderBranchId.selectedIndex].text}, ${genUtil.getDataAttributeValue.dropdownSelected(
        elementInformation.mainFormFields.drpSenderBranchId,
        dataAttributes.branch.address)}
            </div>
              Name and Address of Consignee Branch/Store: ${elementInformation.mainFormFields.drpReceiverBranchId.options[elementInformation
        .mainFormFields.drpReceiverBranchId.selectedIndex].text}, ${genUtil.getDataAttributeValue.dropdownSelected(
            elementInformation.mainFormFields.drpReceiverBranchId,
        dataAttributes.branch.address)}
            </div>
            <div>Driver Name : ${elementInformation.mainFormFields.txtDriverName.value}</div>
            <div>Driver Mobile No. : ${elementInformation.mainFormFields.txtDriverMobile.value}</div>
            <div>Vehicle Type : ${elementInformation.mainFormFields.drpVehicleTypeId.options[elementInformation
        .mainFormFields.drpVehicleTypeId.selectedIndex].text}</div>
            <div>Vehicle Registration No. : ${elementInformation.mainFormFields.txtVehicleRegistrationNo.value}</div>
          </div>

          <div class="col-4 mt-3">
            <div style="margin-top: 3px">Invoice No.: ${elementInformation.mainFormFields.txtInvoiceNo.value}</div>
            <div style="margin-top: 3px">Invoice Date: ${elementInformation.mainFormFields.dateInvoiceDate.value}</div>
            <div>Challan No. : (Challan No. here)</div>
            <div style="margin-top: 3px">Issue Date: (Issue date here)</div>
            <div style="margin-top: 3px">Issue Time: (Issu Time here)</div>
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
                  <td>Unit Price (Taka)</td>
                  <td>Total Price (Taka)</td>
                  <td>Remarks</td>
                </tr>
                <tr class="text-center">
                  <td>(1)</td>
                  <td>(2)</td>
                  <td>(3)</td>
                  <td>(4)</td>
                  <td>(5)</td>
                  <td>(6)</td>
                  <td>(7)</td>
                </tr>
                ${salesDetailsText}
                <tr>
                  <td colspan="5" class="total-text-cell text-right">Total</td>
                  <td class="amount-quantity-cell">${commonUtilitySale.getTotalPrice()}</td>
                  <td></td>
                </tr>
                
              </tbody>
            </table>
          </div> 
      </div>
    </div>
`;
    return draftText;
}