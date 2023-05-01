export function getNameByProps(isDaily, isBudget, isFullTime) {
    if (isDaily && isBudget && isFullTime)
        return "Дневная форма получения образования за счет средств республиканского бюджета";
    else if (!isDaily && isBudget && isFullTime)
        return "Заочная форма получения образования за счет средств республиканского бюджета";
    else if (isDaily && !isBudget && isFullTime)
        return "Дневная форма получения образования на условиях оплаты";
    else if (!isDaily && !isBudget && isFullTime)
        return "Заочная форма получения образования на условиях оплаты";
    if (isDaily && isBudget && !isFullTime)
        return "Дневная форма получения образования за счет средств республиканского бюджета (сокращённый срок)";
    else if (!isDaily && isBudget && !isFullTime)
        return "Заочная форма получения образования за счет средств республиканского бюджета (сокращённый срок)";
    else if (isDaily && !isBudget && !isFullTime)
        return "Дневная форма получения образования на условиях оплаты (сокращённый срок)";
    else if (!isDaily && !isBudget && !isFullTime)
        return "Заочная форма получения образования на условиях оплаты (сокращённый срок)";

    return "";
};