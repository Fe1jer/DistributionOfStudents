export default function SpecialityPlan({ specialityPlan }) {
    return <tr className="align-middle">
        <td>{specialityPlan.specialityName}</td>
        <td>{specialityPlan.dailyFullBudget}</td>
        <td>{specialityPlan.dailyFullPaid}</td>
        <td>{specialityPlan.dailyAbbreviatedBudget}</td>
        <td>{specialityPlan.dailyAbbreviatedPaid}</td>
        <td>{specialityPlan.eveningFullBudget}</td>
        <td>{specialityPlan.eveningFullPaid}</td>
        <td>{specialityPlan.eveningAbbreviatedBudget}</td>
        <td>{specialityPlan.eveningAbbreviatedPaid}</td>
    </tr>;
}