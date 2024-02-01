const listInfo = {
    productProp: {
        productCost: 'ProductCost',
        overheadCost: 'OverheadCostAmount'

    },
    overheadCost: [],
    productCost: [],
    dataListOverHead :[]
}

const dataAttributes = {
    product: {
        measurementUnitId: 'data-product-measurement-unit-id',
        measurementUnitName: 'data-product-measurement-unit-name',
        unitPrice: 'data-product-Unit-Price',
    }
}

const elementInformation = {
    formInfo: {
        frmOverHeadCost: document.getElementById('frmOverHeadCost'),
        frmProductCost: document.getElementById('productCostFrm'),
        frmProfit: document.getElementById('frmProfit'),
    },
    mainFormFields: {
        txtSalesUnitPrice: document.getElementById('SalesUnitPrice'),
        hiddenProductId: document.getElementById('HiddenProductId'),
        hdMeasurementUnitId: document.getElementById('MeasurementUnitId'),
    },
    ProductCostFields: {
        drpProductId: document.getElementById('ProductId'),
        txtRequireQty: document.getElementById('RequiredQty'),
        txtProductCost: document.getElementById('ProductCost'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtWastagePercentage: document.getElementById('WastagePercentage'),
    },
    OverheadCostFields: {
        txtOverheadCostId: document.getElementById('OverheadCostId'),
        txtOverheadCost: document.getElementById('OverheadCostAmount'),

    },
    ProfitFields: {
        txtProfitAmount: document.getElementById('ProfitAmount'),
        txtProfitPercent: document.getElementById('ProfitPercent'),
    },
    btn: {
        btnAddRelatedProductCost: document.getElementById('btnAddRelatedProductCost'),
        btnAddRelatedOverheadCost: document.getElementById('btnAddRelatedOverheadCost'),
    },
    listTables: {
        tbdProductCost: document.getElementById('tbdProductCost'),
        tbdOverheadCost: document.getElementById('tbdOverheadCost'),
    },
    totalOverHeadCostCalculatedTableCell: {
        totalOverHeadCost: document.getElementById('totalOverHeadCost'),
        totalProductCost: document.getElementById('totalProductCost'),
    }
}

elementInformation.ProductCostFields.drpProductId.addEventListener('change', (event) => {
    const drp = event.target;
    const prodDataAttr = dataAttributes.product;

    elementInformation.ProductCostFields.drpMeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);
    elementInformation.ProductCostFields.txtMeasurementUnitName.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);
    elementInformation.ProductCostFields.txtUnitPrice.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.unitPrice);
   
});

const commonUtilityIOC = {

    getTotal: () => {
        debugger;
        let totalOverhead = +elementInformation.totalOverHeadCostCalculatedTableCell.totalOverHeadCost.innerText; 
        let profit = +elementInformation.ProfitFields.txtProfitAmount.value;       
        return (totalOverhead + commonUtilityIOC.getTotalProductCost()) + profit;
    },
    getTotalFinal: () => {
        debugger;
        let totalOverhead = +elementInformation.totalOverHeadCostCalculatedTableCell.totalOverHeadCost.innerText; 
        let totalProductCost = +elementInformation.totalOverHeadCostCalculatedTableCell.totalProductCost.innerText;
        let profit = +elementInformation.ProfitFields.txtProfitAmount.value;
        return (totalOverhead + totalProductCost + profit);
    },
    getProfitToPercent: () => {
        let profitAmount = +elementInformation.ProfitFields.txtProfitAmount.value;
        return generalUtility.roundNumberOption.byFour(((100 * profitAmount) / (+commonUtilityIOC.getTotal())));
    },
    getTotalOverheadCost: () => {
        return generalUtility.getSumFromObjectArray(listInfo.overheadCost, listInfo.productProp.overheadCost);
    },

    getTotalProductCost: () => {
        return generalUtility.getSumFromObjectArray(listInfo.productCost, listInfo.productProp.productCost);
    },
    showTotalOverHeadCost: () => {
        elementInformation.totalOverHeadCostCalculatedTableCell.totalOverHeadCost.innerText = generalUtility.roundNumberOption.byFour(commonUtilityIOC.getTotalOverheadCost());
    },
    showTotalProductCost: () => {
        elementInformation.totalOverHeadCostCalculatedTableCell.totalProductCost.innerText = generalUtility.roundNumberOption.byFour(commonUtilityIOC.getTotalProductCost());
    },
    showTotal: () => {
        elementInformation.mainFormFields.txtSalesUnitPrice.value = generalUtility.roundNumberOption.byFour(commonUtilityIOC.getTotal());
    },
    showTotalFinal: () => {
        elementInformation.mainFormFields.txtSalesUnitPrice.value = generalUtility.roundNumberOption.byFour(commonUtilityIOC.getTotalFinal());
    },
    showTotalPercent: () => {
        elementInformation.ProfitFields.txtProfitPercent.value = generalUtility.roundNumberOption.byFour(commonUtilityIOC.getProfitToPercent());
    },
    costCalByQty: (unitPrice, qty) => {
        return unitPrice * qty;
    },
    qtyCalByCost: (costPrice, unitPrice) => {
        return costPrice / unitPrice;
    }
}

const commonChangeEventsForCalculation = {
    resetForm: {
        overHeadCost: () => {
            elementInformation.formInfo.frmOverHeadCost.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.OverheadCostFields.txtOverheadCostId);
        },
        productCost: () => {
            $('#ProductCost').val(0);
            $('.unitPrice').val(0);
            $('#RequiredQty').val(0);
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.ProductCostFields.drpProductId);
        }
    },
    QtyChangeEvent: (unitPrice, qty) => {
        elementInformation.ProductCostFields.txtProductCost.value = generalUtility.roundNumberOption.byFour(commonUtilityIOC.costCalByQty(unitPrice, qty));
    },
    CostChangeEvent: (costPrice, unitPrice) => {
        elementInformation.ProductCostFields.txtRequireQty.value = generalUtility.roundNumberOption.byFour(commonUtilityIOC.qtyCalByCost(costPrice, unitPrice));
    }

}

elementInformation.ProductCostFields.txtRequireQty.addEventListener('input', (event) => {
    if (elementInformation.ProductCostFields.drpProductId.value) {
        const requireQty = +event.target.value;
        const unitPrice = +(elementInformation.ProductCostFields.txtUnitPrice.value);
        commonChangeEventsForCalculation.QtyChangeEvent(unitPrice, requireQty);
        console.log(elementInformation.ProductCostFields.txtProductCost.value);
    } else {
        elementInformation.ProductCostFields.txtRequireQty.value = 0;
        toastr.error('Please Select Product First');
    }
});

$(document).on('input', '#OvHeadCost', function (event) {
    
    debugger;
    function calculateTotaOverHeadlValue() {
        var total = 0;
        $('.ovhdcost').each(function () {
            var value = parseFloat($(this).val()) || 0;
            total += value;
        });
        $('#totalOverHeadCost').text(total.toFixed(2)); 
    }
    calculateTotaOverHeadlValue();
    $('.ovhdcost').on('input', function () {
        calculateTotaOverHeadlValue(); // Recalculate total when input values change
    });
    commonUtilityIOC.showTotalFinal();
});

elementInformation.ProductCostFields.txtProductCost.addEventListener('input', (event) => {
    //if (elementInformation.ProductCostFields.drpProductId.value) {
    //    const cost = +event.target.value;
    //    const unitPrice = +(elementInformation.ProductCostFields.txtUnitPrice.value);
    //    commonChangeEventsForCalculation.CostChangeEvent(cost, unitPrice);
    //    console.log(elementInformation.ProductCostFields.txtRequireQty.value);
    //} else {
    //    elementInformation.ProductCostFields.txtProductCost.value = 0;
    //    toastr.error('Please Select Product First');
    //}
});

elementInformation.ProfitFields.txtProfitAmount.addEventListener('input', (event) => {
    const profitAmount = +event.target.value;
    if (profitAmount && (commonUtilityIOC.getTotal()) > 0) {
        commonUtilityIOC.showTotalPercent();
        commonUtilityIOC.showTotalFinal();
    } else {
        elementInformation.ProfitFields.txtProfitPercent.value = 0;
        commonUtilityIOC.showTotalFinal();
    }
});

elementInformation.btn.btnAddRelatedProductCost.addEventListener('click', () => {

    if (!isProductExit(elementInformation.ProductCostFields.drpProductId.value)) {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmProductCost))
            addRelatedProductCosts();
    } else {
        toastr.error('Product Already Exist');
    }
});

//Add Product Cost Table Row
const addRelatedProductCosts = () => {
    debugger;
    const ProductName = elementInformation.ProductCostFields.drpProductId.options[elementInformation.ProductCostFields.drpProductId.selectedIndex].text;
    const newProductCostRndId = Math.random().toString(36).substring(2);
    const newProductCostInfo = {
        RndId: newProductCostRndId,
        ProductId: elementInformation.ProductCostFields.drpProductId.value,
        RequireQty: elementInformation.ProductCostFields.txtRequireQty.value,
        ProductCost: +(elementInformation.ProductCostFields.txtProductCost.value),
        UnitPrice: elementInformation.ProductCostFields.txtUnitPrice.value,
        MeasurementUnitId: elementInformation.ProductCostFields.drpMeasurementUnitId.value,
        WastagePercentage: elementInformation.ProductCostFields.txtWastagePercentage.value,

    }
    listInfo.productCost.push(newProductCostInfo);
    const newProductCostDataRow = `
        <tr id="row_${newProductCostRndId}">
            <td>${ProductName}</td>
            <td class="text-right"><input type="text" class="form-control form-control-sm" id="txtRequireQty_${newProductCostRndId}" value="${elementInformation.ProductCostFields.txtRequireQty.value}" /></td>
            <td class="text-right"><input type="text" class="form-control form-control-sm" id="txtProductCost_${newProductCostRndId}" value="${elementInformation.ProductCostFields.txtProductCost.value}" /></td>
            <td class="text-right"><input type="text" class="form-control form-control-sm" id="txtUnitPrice_${newProductCostRndId}" value="${elementInformation.ProductCostFields.txtUnitPrice.value}" /></td>     
            <td class="text-center">${elementInformation.ProductCostFields.txtMeasurementUnitName.value}</td>
            <td class="text-right">${elementInformation.ProductCostFields.txtWastagePercentage.value}</td>
            <td class="text-center">
 <a onclick="removeRelatedProductCost('${newProductCostRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    elementInformation.listTables.tbdProductCost.insertAdjacentHTML('beforeend', newProductCostDataRow);

    //Add listener for new input row changes
    const inputRequireQty = document.getElementById(`txtRequireQty_${newProductCostRndId}`);
    inputRequireQty.addEventListener('input', handleInputChangeInReqQt);
    const inputProductCost = document.getElementById(`txtProductCost_${newProductCostRndId}`);
    inputProductCost.addEventListener('input', handleInputChangeInProductCost);

    commonChangeEventsForCalculation.resetForm.productCost();
    const rows = document.querySelectorAll('[id^="row_"]');
    let total = 0;
    rows.forEach(row => {
        const totalCost3 = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
        total += totalCost3;
    });
    $('#totalProductCost').text(total);
    commonUtilityIOC.showTotalFinal();
    commonUtilityIOC.showTotalPercent();
}
function handleInputChangeInReqQt(event) {
    debugger;
    let dataList = [];
    const id = event.target.id.split('_')[1]; 
    const row = event.target.closest('tr');
    const requireQty = parseFloat(row.querySelector('[id^="txtRequireQty_"]').value) || 0;
    const unitPrice = parseFloat(row.querySelector('[id^="txtUnitPrice_"]').value) || 0;
    const productCost = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
    // Find existing entry in dataList or create a new one
    const existingEntryIndex = dataList.findIndex(item => item.id === id);
    if (existingEntryIndex !== -1) {
        dataList[existingEntryIndex] = {
            id: id,
            requireQty: requireQty,
            productCost: productCost,
            unitPrice: unitPrice
        };
    } else {
        dataList.push({
            id: id,
            requireQty: requireQty,
            productCost: productCost,
            unitPrice: unitPrice
        });
    }
    updateTotalCostInTable();
    function updateTotalCostInTable() {
        let total = 0;
        const totalCost = requireQty * unitPrice;
        row.querySelector('[id^="txtProductCost_"]').value = totalCost.toFixed(2);

        const rows = document.querySelectorAll('[id^="row_"]');
        rows.forEach(row => {
            const totalCost3 = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
            total += totalCost3;
        });
        $('#totalProductCost').text(total);
    }
    commonUtilityIOC.showTotalFinal();
   // updateTotalCostInTable();
}
function handleInputChangeInProductCost(event) {
    debugger;
    const row = event.target.closest('tr');
    const productCost = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
    const rows = document.querySelectorAll('[id^="row_"]');
    let total = 0;
    rows.forEach(row => {
        const totalCost1 = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
        total += totalCost1;
    });
    $('#totalProductCost').text(total);
    commonUtilityIOC.showTotalFinal();
}

const removeRelatedProductCost = (newProductCostRndId) => {
    const productCostDisplayRowId = document.getElementById(`row_${newProductCostRndId}`);
    productCostDisplayRowId.remove();
    listInfo.productCost = listInfo.productCost.filter(f => f.RndId !== newProductCostRndId);

    //commonUtilityIOC.showTotalProductCost();
    const rows = document.querySelectorAll('[id^="row_"]');
    let total = 0;
    rows.forEach(row => {
        const totalCost3 = parseFloat(row.querySelector('[id^="txtProductCost_"]').value) || 0;
        total += totalCost3;
    });
    $('#totalProductCost').text(total);
    commonChangeEventsForCalculation.resetForm.productCost();
    commonUtilityIOC.showTotalFinal();
    commonUtilityIOC.showTotalPercent();
}

//Add Over Head Cost Table Row
const addRelatedOverheadCosts = () => {
    const OverheadCostName = elementInformation.OverheadCostFields.txtOverheadCostId.options[elementInformation.OverheadCostFields.txtOverheadCostId.selectedIndex].text;
    const newOverheadRndId = Math.random().toString(36).substring(2);
    const newOverheadCostInfo = {
        RndId: newOverheadRndId,
        OverheadCostId: elementInformation.OverheadCostFields.txtOverheadCostId.value,
        OverheadCostAmount: +(elementInformation.OverheadCostFields.txtOverheadCost.value),
    }
    //listInfo.overheadCost.push(newOverheadCostInfo);
    const newOverheadCostDataRow = `
        <tr id="row_${newOverheadRndId}">
            <td>${OverheadCostName}</td>
            <td class="text-right">${elementInformation.OverheadCostFields.txtOverheadCost.value}</td>
            <td class="text-center">
 <a onclick="removeRelatedOverhead('${newOverheadRndId}')" class="btn btn-danger btn-sm" aria-label="Left Align"><i class="bi bi-trash"></i></a></td></tr>`;
    elementInformation.listTables.tbdOverheadCost.insertAdjacentHTML('beforeend', newOverheadCostDataRow);
    commonUtilityIOC.showTotalOverHeadCost();
    commonChangeEventsForCalculation.resetForm.overHeadCost();
    commonUtilityIOC.showTotal();
    commonUtilityIOC.showTotalPercent();
}

const removeRelatedOverhead = (newOverheadRndId) => {
    const productCostDisplayRowId = document.getElementById(`row_${newOverheadRndId}`);
    productCostDisplayRowId.remove();
   // listInfo.overheadCost = listInfo.overheadCost.filter(f => f.RndId !== newOverheadRndId);
    commonUtilityIOC.showTotalOverHeadCost();
    commonUtilityIOC.showTotal();
    commonUtilityIOC.showTotalPercent();
}

const isProductExit = (productId) => {
    if (listInfo.productCost.find(f => f.ProductId == productId)) {
        return true;
    }
    return false;
}
const isOverheadExit = (overHeadId) => {
    if (listInfo.overheadCost.find(f => f.OverheadCostId == overHeadId)) {
        return true;
    }
    return false;
}
//saveData Request Start
const saveData = (data) => {
    $.ajax({
        url: 'InputOutputCoEfficient',
        data: data,
        cache: false,
        method: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function (d) {
            window.location.href = `/Products/Details/${d.id}`;
            //window.location.href = `Details/${d.id}`;
        },
        error: function () {
            // viewErrorFeedBack();
            //hideContentById('loaderAnimation');
        }
    });
}
//saveData Request End

//Start Make Form Data to Object
const sendToInputOutputCoEfficientController = () => {
    debugger;
    if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmProfit)) {
        let formData = new FormData();
        let ProductCostIndex = 0;
        let OverheadCostIndex = 0;

        formData.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
        formData.append("SalesUnitPrice", elementInformation.mainFormFields.txtSalesUnitPrice.value);
        formData.append("HiddenProductId", elementInformation.mainFormFields.hiddenProductId.value);
        formData.append("ProfitAmount", elementInformation.ProfitFields.txtProfitAmount.value);
        formData.append("ProfitPercent", elementInformation.ProfitFields.txtProfitPercent.value);
        formData.append("MeasurementUnitId", elementInformation.mainFormFields.hdMeasurementUnitId.value);

        for (const relatedProductCost of listInfo.productCost) {
            formData.append(`ProductCostList[${ProductCostIndex}].ProductId`, relatedProductCost.ProductId);
            formData.append(`ProductCostList[${ProductCostIndex}].RequiredQty`, relatedProductCost.RequireQty);
            formData.append(`ProductCostList[${ProductCostIndex}].ProductCost`, relatedProductCost.ProductCost);
            formData.append(`ProductCostList[${ProductCostIndex}].UnitPrice`, relatedProductCost.UnitPrice);
            formData.append(`ProductCostList[${ProductCostIndex}].MeasurementUnitId`, relatedProductCost.MeasurementUnitId);
            formData.append(`ProductCostList[${ProductCostIndex}].WastagePercentage`, relatedProductCost.WastagePercentage);
            ProductCostIndex++;
        }
        listOverHeadCost()
           
        for (const relatedOverheadCost of listInfo.dataListOverHead) {
            if (relatedOverheadCost.OverheadCostAmount > 0) {
                formData.append(`OverheadCostList[${OverheadCostIndex}].OverheadCostId`, relatedOverheadCost.OverheadCostId);
                formData.append(`OverheadCostList[${OverheadCostIndex}].OverheadCostAmount`, relatedOverheadCost.OverheadCostAmount);
                OverheadCostIndex++;
            }
        }
        saveData(formData);
    }

    function listOverHeadCost() {
      $('#tbdOverheadCost tr').each(function () {
            var newOverheadRndId = Math.random().toString(36).substring(2);
            var OverheadCostId = $(this).find('#OverheadCostId').text();
            var OverheadCostAmount = $(this).find('.ovhdcost').val();
            var dataObject = {
                RndId: newOverheadRndId,
                OverheadCostId: OverheadCostId,
                OverheadCostAmount: OverheadCostAmount
            }
            listInfo.dataListOverHead.push(dataObject);
        });
    }
}

//Start Make Form Data to Object