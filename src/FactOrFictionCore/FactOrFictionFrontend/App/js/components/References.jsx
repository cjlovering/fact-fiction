import * as React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

export default class References extends React.Component {
    static propTypes = {
        references: PropTypes.array.isRequired,
        cleanLink: PropTypes.func.isRequired
    }

    render() {
        let { references, cleanLink } = this.props;

        return (
            references.map(ref => (
                <tr key={shortid.generate()} style={{width: "100%"}}>
                    <th>
                        <a href={ref.link} > {cleanLink(ref.link)} </a>
                    </th>
                    <th>
                        {ref.bias}
                    </th>
                </tr>
            ))        
        );
    }
}