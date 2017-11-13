import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import FactCard from './FactCard';
import _ from '../../stylesheets/components/_ListView.scss';

export default class ListView extends React.Component {
    render() {
        const { entries, selectedEntryId, selectEntry } = this.props;

        return (
            <div className='list-view'>
                {entries
                    .filter(entry => entry.type == "OBJECTIVE")
                    .map(entry => (
                        <FactCard
                            {...entry}
                            selectedEntryId={selectedEntryId}
                            selectEntry={selectEntry}
                            key={shortid.generate()}
                        />
                        )
                    )}
            </div>
        );
    }
}

ListView.propTypes = {
    entries: PropTypes.array.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired
}