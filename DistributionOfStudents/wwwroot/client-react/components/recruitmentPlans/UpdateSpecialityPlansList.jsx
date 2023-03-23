import UpdateSpecialityPlan from "./UpdateSpecialityPlan.jsx";

export default function UpdateSpecialityPlansList({ plans, errors, onChange }) {
    const [changedPlans, setChangedPlans] = React.useState(plans);
    const onChangePlan = (plan, index) => {
        var changedPlansTemp = changedPlans;
        changedPlansTemp[index] = plan;
        setChangedPlans(changedPlansTemp);
        onChange(changedPlans);
    }
    return <table className="table table-bordered mb-0">
        <thead>
            <tr>
                <th rowSpan="2">Специальность (направление специальности)</th>
                <th colSpan="4">Дневная форма получения образования</th>
                <th colSpan="4">Заочная форма получения образования</th>
            </tr>
            <tr>
                <th width="85">Бюджет</th>
                <th width="85">Платное</th>
                <th width="85">Бюджет сокр.срок</th>
                <th width="85">Платное сокр.срок</th>
                <th width="85">Бюджет</th>
                <th width="85">Платное</th>
                <th width="85">Бюджет сокр.срок</th>
                <th width="85">Платное сокр.срок</th>
            </tr>
        </thead>
        <tbody>{
            plans.map((item, index) =>
                <UpdateSpecialityPlan key={JSON.stringify(item)} index={index} specialityPlan={item} errors={errors} onChange={onChangePlan} />
            )}
        </tbody>
    </table>;
}