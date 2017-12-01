import * as React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

export default class Entities extends React.Component {
    static propTypes = {
        entities: PropTypes.array.isRequired
    }

    render() {
        const { entities } = this.props;
        return entities.map(entity => (
                <tr key={shortid.generate()} style={{width: "100%"}}>
                    <td>
                        <a href={entity.wikiUrl} > {entity.name} </a>
                    </td>
                </tr>
            )
        );
    }
}