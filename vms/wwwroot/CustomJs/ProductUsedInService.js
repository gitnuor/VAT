/// <reference path="site.js" />

const genUtil = window.generalUtility;

//Assign Form Related id here Like btn,Field,Table id,Table Body Id,etc
const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
    },
    mainFormFields: {
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        drpCustomerId: document.getElementById('CustomerId'),
        drpProductId: document.getElementById('ProductId'),
        drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtCurrentStock: document.getElementById('CurrentStock'),
        txtQuantity: document.getElementById('Quantity'),
    },
    btn: {
        btnSave: document.getElementById('btnSave'),
        btnResetForm: document.getElementById('btnResetForm')
    },
}

const commonUtilitySale = {
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
    getProductsForSale: async (branchId) => {
        const url = `/api/salesproducts/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
    getCustomerByBranch: async (branchId) => {
        const url = `/api/customerbybranch/${branchId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

const dataAttributes = {
    product: {
        productMaxSaleQty: 'data-product-max-sale-quantity',
        measurementUnitId: 'data-product-measurement-unit-id',
        measurementUnitName: 'data-product-measurement-unit-name',
    },
    productMeasurement: {
        productMeasurementConvertionRatio: 'data-product-convertion-ratio'
    },
}

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
                            data-product-max-sale-quantity='${element.maxSaleQty}'
                            data-product-measurement-unit-id='${element.measurementUnitId}'
                            data-product-measurement-unit-name='${element.measurementUnitName}'>
                        ${element.productDescription}
                    </option>
                `;
                });
                genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpProductId,
                    prodDrpElements);

            });

            commonUtilitySale.getCustomerByBranch(branchId).then(brnc => {
                brnc.forEach(element => {
                    custDrpElements += `
                    <option value='${element.customerId}'>
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

    });

elementInformation.mainFormFields.drpProductId.addEventListener('change',
    (event) => {
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
                genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpMeasurementUnitId,
                    unitDrpElements);
            }).then(() => {
                setProductInfoByProductId(event);
            });

        } else {
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.mainFormFields.drpMeasurementUnitId,
                unitDrpElements);
        }
    });

//Set Product Related Field Method
const setProductInfoByProductId = (event) => {

    const drp = event.target;
    commonChangeEventsForCalculation.clearStockAndQuantityEvent();

    const prodDataAttr = dataAttributes.product;
    const prodFld = elementInformation.mainFormFields;
    prodFld.txtCurrentStock.value = genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.productMaxSaleQty);
    genUtil.setDropdownValue.selectPickerByControl(prodFld.drpMeasurementUnitId,
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId));

}

const commonChangeEventsForCalculation = {
    clearStockAndQuantityEvent: () => {
        elementInformation.mainFormFields.txtCurrentStock.value = 0;
        elementInformation.mainFormFields.txtQuantity.value = 0;
    },
    clearQuantityEvent: () => {
        elementInformation.mainFormFields.txtQuantity.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmMainInformation.reset();
            genUtil.setDropdownValue.selectPickerReset(elementInformation.mainFormFields.drpProductId);
            genUtil.setDropdownValue.selectPickerReset(elementInformation.mainFormFields.drpCustomerId);
            genUtil.setDropdownValue.selectPickerReset(elementInformation.mainFormFields.drpMeasurementUnitId);
        },
    },
}

elementInformation.mainFormFields.drpMeasurementUnitId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        const measurementDataAttr = dataAttributes.productMeasurement;
        commonChangeEventsForCalculation.clearQuantityEvent();
        const prodFld = elementInformation.mainFormFields;
        prodFld.txtCurrentStock.value = (
            (+genUtil.getDataAttributeValue.dropdownSelected(elementInformation.mainFormFields.drpProductId, prodDataAttr.productMaxSaleQty))
            *
            (+genUtil.getDataAttributeValue.dropdownSelected(drp, measurementDataAttr.productMeasurementConvertionRatio))
        );
    });

elementInformation.mainFormFields.txtQuantity.addEventListener('change',
    (event) => {
        const quantityTextBox = event.target;
        const quantity = +quantityTextBox.value;
        const currentStock = +elementInformation.mainFormFields.txtCurrentStock.value;
        if (quantity > currentStock) {
            genUtil.message.showErrorMsg("Quantity should be less than or equal to current stock !!");
            commonChangeEventsForCalculation.clearQuantityEvent();
        }
        
    });
