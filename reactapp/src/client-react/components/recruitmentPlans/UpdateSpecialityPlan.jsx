import React from 'react';
import Form from 'react-bootstrap/Form';

export default function UpdateSpecialityPlan({ specialityPlan, errors, handleChange, index }) {
    return (    
        <tr className="align-middle">
            <td>{specialityPlan.specialityName}</td>
            <td><Form.Control type="number" name={"plans[" + index + "].dailyFullBudget"} value={specialityPlan.dailyFullBudget}
                onChange={handleChange}
                isInvalid={!!errors.dailyFullBudget} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].dailyFullPaid"} value={specialityPlan.dailyFullPaid}
                onChange={handleChange}
                isInvalid={!!errors.dailyFullPaid} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].dailyAbbreviatedBudget"} value={specialityPlan.dailyAbbreviatedBudget}
                onChange={handleChange}
                isInvalid={!!errors.dailyAbbreviatedBudget} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].dailyAbbreviatedPaid"} value={specialityPlan.dailyAbbreviatedPaid}
                onChange={handleChange}
                isInvalid={!!errors.dailyAbbreviatedPaid} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].eveningFullBudget"} value={specialityPlan.eveningFullBudget}
                onChange={handleChange}
                isInvalid={!!errors.eveningFullBudget} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].eveningFullPaid"} value={specialityPlan.eveningFullPaid}
                onChange={handleChange}
                isInvalid={!!errors.eveningFullPaid} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].eveningAbbreviatedBudget"} value={specialityPlan.eveningAbbreviatedBudget}
                onChange={handleChange}
                isInvalid={!!errors.eveningAbbreviatedBudget} /></td>
            <td><Form.Control type="number" name={"plans[" + index + "].eveningAbbreviatedPaid"} value={specialityPlan.eveningAbbreviatedPaid}
                onChange={handleChange}
                isInvalid={!!errors.eveningAbbreviatedPaid} /></td>
        </tr>
    );
}