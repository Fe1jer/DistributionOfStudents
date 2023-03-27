import React from 'react';

export default function UpdateSpecialityPlan({ specialityPlan, errors, onChange, index }) {
    const [formValue, setFormValue] = React.useState({
        dailyFullBudget: specialityPlan.dailyFullBudget,
        dailyFullPaid: specialityPlan.dailyFullPaid,
        dailyAbbreviatedBudget: specialityPlan.dailyAbbreviatedBudget,
        dailyAbbreviatedPaid: specialityPlan.dailyAbbreviatedPaid,
        eveningFullBudget: specialityPlan.eveningFullBudget,
        eveningFullPaid: specialityPlan.eveningFullPaid,
        eveningAbbreviatedBudget: specialityPlan.eveningAbbreviatedBudget,
        eveningAbbreviatedPaid: specialityPlan.eveningAbbreviatedPaid,
        specialityName: specialityPlan.specialityName,
        specialityId: specialityPlan.specialityId,
    });
    const {
        specialityName, specialityId,
        dailyFullBudget, dailyFullPaid, dailyAbbreviatedBudget, dailyAbbreviatedPaid,
        eveningFullBudget, eveningFullPaid, eveningAbbreviatedBudget, eveningAbbreviatedPaid
    } = formValue;

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormValue((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    };

    React.useEffect(() => {
        onChange(formValue, index);
    }, [formValue])
    return (
        <tr className="align-middle">
            <td>{specialityName}</td>
            <td><input min="0" className="form-control" type="number" name="dailyFullBudget" onChange={handleChange} value={dailyFullBudget} /></td>
            <td><input min="0" className="form-control" type="number" name="dailyFullPaid" onChange={handleChange} value={dailyFullPaid} /></td>
            <td><input min="0" className="form-control" type="number" name="dailyAbbreviatedBudget" onChange={handleChange} value={dailyAbbreviatedBudget} /></td>
            <td><input min="0" className="form-control" type="number" name="dailyAbbreviatedPaid" onChange={handleChange} value={dailyAbbreviatedPaid} /></td>
            <td><input min="0" className="form-control" type="number" name="eveningFullBudget" onChange={handleChange} value={eveningFullBudget} /></td>
            <td><input min="0" className="form-control" type="number" name="eveningFullPaid" onChange={handleChange} value={eveningFullPaid} /></td>
            <td><input min="0" className="form-control" type="number" name="eveningAbbreviatedBudget" onChange={handleChange} value={eveningAbbreviatedBudget} /></td>
            <td><input min="0" className="form-control" type="number" name="eveningAbbreviatedPaid" onChange={handleChange} value={eveningAbbreviatedPaid} /></td>
        </tr>
    );
}