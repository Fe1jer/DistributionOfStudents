import SpecialityArchive from './SpecialityArchive.jsx';

export default function SpecialitiesArchiveList({ groupArchive }) {
    const _getCountOfSpecialities = () => {
        var count = groupArchive.recruitmentPlans.length;
        count = count + groupArchive.recruitmentPlans.filter(i => i.target > 0).length;
        return count;
    }

    return (
        groupArchive.recruitmentPlans.map((specialityArchive, number) =>
            <SpecialityArchive key={JSON.stringify(specialityArchive)} number={number} specialityArchive={specialityArchive}
                countOfSpecialities={_getCountOfSpecialities()} competition={groupArchive.competition} />
        ));
}