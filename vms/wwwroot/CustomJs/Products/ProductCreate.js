let vatTypeId = document.getElementById('ProductVattypeId');
let productDefaultVatPercent = document.getElementById('ProductDefaultVatPercent');
vatTypeId.addEventListener('change', (event) => {
    const drp = event.target;
    const isVatUpdateable = generalUtility.getDataAttributeValue.dropdownSelected(drp, 'data-is-vat-updatable');
    const vatPercent = generalUtility.getDataAttributeValue.dropdownSelected(drp, 'data-product-default-vat-percent');
    console.log(vatPercent);

    if (isVatUpdateable === 'True') {
        productDefaultVatPercent.value = vatPercent;
        generalUtility.alterAttr.removeAttr(productDefaultVatPercent, "readonly");
    }
    else {
        productDefaultVatPercent.value = vatPercent;
        generalUtility.alterAttr.setAttr(productDefaultVatPercent, "readonly", "readonly");
    }

});