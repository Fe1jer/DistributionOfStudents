import SpecialityArchive from './SpecialityArchive.jsx';

export default function SpecialitiesArchiveList({ groupArchive }) {
    return (
        groupArchive.recruitmentPlans.map((specialityArchive, number) =>
            <SpecialityArchive key={JSON.stringify(specialityArchive)} number={number} specialityArchive={specialityArchive} countOfSpecialities={groupArchive.recruitmentPlans.length} competition={groupArchive.competition} />
        ));
}