/// <reference path="site.js" />

const genUtil = window.generalUtility;

//Assign Form Related id here Like btn,Field,Table id,Table Body Id,etc
const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
    },
    mainFormFields: {
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        txtReceiveDate: document.getElementById('ReceiveDate'),
        drpProductId: document.getElementById('ProductId'),
        drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        txtQuantity: document.getElementById('Quantity'),
        txtProductDescription: document.getElementById('ProductDescription'),
        txtRemarks: document.getElementById('Remarks'),
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

const setProductInfoByProductId = (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;
    const prodFld = elementInformation.mainFormFields;
    genUtil.setDropdownValue.selectPickerByControl(prodFld.drpMeasurementUnitId,
        genUtil.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId));

}